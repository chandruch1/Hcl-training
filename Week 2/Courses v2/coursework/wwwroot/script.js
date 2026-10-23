const API_BASE_URL = "http://localhost:5000";

const API = {
    courses: `${API_BASE_URL}/api/courses`,
    learners: `${API_BASE_URL}/api/learners`,
    enrollments: `${API_BASE_URL}/api/enrollments`,
    payments: `${API_BASE_URL}/api/payments`
};

async function apiRequest(url, options = {}) {
    const response = await fetch(url, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...options.headers
        }
    });

    if (!response.ok) {
        const message = await response.text();
        throw new Error(
            `HTTP ${response.status}: ${message || response.statusText}`
        );
    }

    if (response.status === 204) {
        return null;
    }

    const contentType = response.headers.get("content-type") || "";

    if (contentType.includes("application/json")) {
        return response.json();
    }

    return null;
}


function cell(row, value) {
  const td = document.createElement("td");
  td.textContent = value ?? "";
  row.appendChild(td);
  return td;
}

function addButton(row, text, action, danger = false) {
  const td = document.createElement("td");
  const button = document.createElement("button");
  button.textContent = text;
  if (danger) button.classList.add("danger");
  button.addEventListener("click", action);
  td.appendChild(button);
  row.appendChild(td);
}

function money(value) {
  return new Intl.NumberFormat("en-IN", { style: "currency", currency: "INR" }).format(value);
}

function dateOnly(value) {
  return value ? new Date(value).toLocaleDateString("en-IN") : "";
}

function dateTime(value) {
  return value ? new Date(value).toLocaleString("en-IN") : "";
}

async function loadCourses() {
  const courses = await request(api.courses);
  const tbody = $("coursesTable");
  tbody.replaceChildren();
  const select = $("enrollCourseCode");
  const oldValue = select.value;
  select.replaceChildren(new Option("Select course", ""));
  courses.forEach(c => {
    const row = document.createElement("tr");
    cell(row, c.courseCode);
    cell(row, c.title);
    cell(row, money(c.fee));
    cell(row, c.durationDays);
    cell(row, c.totalSeats);
    cell(row, c.availableSeats ?? (c.totalSeats - c.enrolledCount));
    addButton(row, "Delete", async () => {
      if (!confirm(`Delete course ${c.courseCode}?`)) return;
      try {
        await request(`${api.courses}/${encodeURIComponent(c.courseCode)}`, { method: "DELETE" });
        showMessage("Course deleted.");
        await refreshAll();
      } catch (e) { showMessage(e.message, "error"); }
    }, true);
    tbody.appendChild(row);
    select.add(new Option(`${c.courseCode} — ${c.title}`, c.courseCode));
  });
  select.value = oldValue;
}

async function loadLearners() {
  const learners = await request(api.learners);
  const tbody = $("learnersTable");
  tbody.replaceChildren();
  learners.forEach(l => {
    const row = document.createElement("tr");
    cell(row, l.learnerId);
    cell(row, l.learnerName);
    cell(row, l.email);
    cell(row, l.contact);
    tbody.appendChild(row);
  });
}

async function loadEnrollments() {
  const enrollments = await request(api.enrollments);
  const tbody = $("enrollmentsTable");
  tbody.replaceChildren();
  enrollments.forEach(e => {
    const row = document.createElement("tr");
    cell(row, e.enrollmentId);
    cell(row, e.learnerId);
    cell(row, e.courseCode);
    cell(row, dateOnly(e.enrollmentDate));
    cell(row, money(e.finalFee));
    cell(row, e.status);
    cell(row, e.paymentStatus);
    if (e.status !== "Cancelled") {
      addButton(row, "Cancel", async () => {
        if (!confirm(`Cancel enrollment ${e.enrollmentId}?`)) return;
        try {
          const result = await request(`${api.enrollments}/${e.enrollmentId}`, { method: "DELETE" });
          showMessage(result?.message || "Enrollment cancelled.");
          await refreshAll();
        } catch (err) { showMessage(err.message, "error"); }
      }, true);
    } else {
      cell(row, "—");
    }
    tbody.appendChild(row);
  });
}

async function loadPayments() {
  const payments = await request(api.payments);
  const tbody = $("paymentsTable");
  tbody.replaceChildren();
  payments.forEach(p => {
    const row = document.createElement("tr");
    cell(row, p.paymentId);
    cell(row, p.enrollmentId);
    cell(row, money(p.amount));
    cell(row, p.mode);
    cell(row, p.status);
    cell(row, dateTime(p.paymentDate));
    tbody.appendChild(row);
  });
}

async function refreshAll() {
  try {
    await Promise.all([loadCourses(), loadLearners(), loadEnrollments(), loadPayments()]);
  } catch (e) {
    showMessage(`${e.message}. Make sure the ASP.NET Core project is running.`, "error");
  }
}

$("courseForm").addEventListener("submit", async event => {
  event.preventDefault();
  try {
    await request(api.courses, {
      method: "POST",
      body: JSON.stringify({
        courseCode: $("courseCode").value.trim(),
        title: $("courseTitle").value.trim(),
        fee: Number($("courseFee").value),
        durationDays: Number($("courseDuration").value),
        totalSeats: Number($("courseSeats").value)
      })
    });
    event.target.reset();
    showMessage("Course added successfully.");
    await refreshAll();
  } catch (e) { showMessage(e.message, "error"); }
});

$("learnerForm").addEventListener("submit", async event => {
  event.preventDefault();
  try {
    await request(api.learners, {
      method: "POST",
      body: JSON.stringify({
        learnerId: Number($("learnerId").value),
        learnerName: $("learnerName").value.trim(),
        email: $("learnerEmail").value.trim(),
        contact: Number($("learnerContact").value)
      })
    });
    event.target.reset();
    showMessage("Learner registered successfully.");
    await loadLearners();
  } catch (e) { showMessage(e.message, "error"); }
});

$("enrollmentForm").addEventListener("submit", async event => {
  event.preventDefault();
  try {
    await request(api.enrollments, {
      method: "POST",
      body: JSON.stringify({
        learnerId: Number($("enrollLearnerId").value),
        courseCode: $("enrollCourseCode").value
      })
    });
    event.target.reset();
    showMessage("Enrollment created successfully.");
    await refreshAll();
  } catch (e) { showMessage(e.message, "error"); }
});

$("paymentForm").addEventListener("submit", async event => {
  event.preventDefault();
  try {
    const result = await request(api.payments, {
      method: "POST",
      body: JSON.stringify({
        enrollmentId: Number($("paymentEnrollmentId").value),
        mode: $("paymentMode").value
      })
    });
    showMessage(`Payment attempt recorded: ${result.status}.`);
    await refreshAll();
  } catch (e) { showMessage(e.message, "error"); }
});

document.querySelectorAll(".tab").forEach(button => {
  button.addEventListener("click", () => {
    document.querySelectorAll(".tab").forEach(b => b.classList.toggle("active", b === button));
    document.querySelectorAll(".panel").forEach(panel =>
      panel.classList.toggle("hidden", panel.id !== button.dataset.panel));
  });
});

$("refreshCourses").addEventListener("click", () => loadCourses().catch(e => showMessage(e.message, "error")));
$("refreshLearners").addEventListener("click", () => loadLearners().catch(e => showMessage(e.message, "error")));
$("refreshEnrollments").addEventListener("click", () => loadEnrollments().catch(e => showMessage(e.message, "error")));
$("refreshPayments").addEventListener("click", () => loadPayments().catch(e => showMessage(e.message, "error")));

refreshAll();
