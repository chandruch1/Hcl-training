
const API_URL = "/api/course";

const form = document.getElementById("courseForm");
const table = document.getElementById("courseTable");
const message = document.getElementById("message");

async function loadCourses() {
    try {
        const response = await fetch(API_URL);

        if (!response.ok) {
            throw new Error("Could not load courses.");
        }

        const courses = await response.json();

        table.replaceChildren();

        courses.forEach(course => {
            const row = document.createElement("tr");

            const values = [
                course.courseCode,
                course.title,
                course.fee,
                course.durationDays,
                course.totalSeats - course.enrolledCount
            ];

            values.forEach(value => {
                const cell = document.createElement("td");
                cell.textContent = value;
                row.appendChild(cell);
            });

            table.appendChild(row);
        });
    } catch (error) {
        message.textContent = error.message;
    }
}

form.addEventListener("submit", async event => {
    event.preventDefault();

    const course = {
        courseCode: document.getElementById("courseCode").value,
        title: document.getElementById("title").value,
        fee: Number(document.getElementById("fee").value),
        durationDays: Number(document.getElementById("duration").value),
        totalSeats: Number(document.getElementById("seats").value)
    };

    try {
        const response = await fetch(API_URL, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(course)
        });

        const result = await response.json();

        if (!response.ok) {
            throw new Error(result.message || "Failed to add course.");
        }

        message.textContent = "Course added successfully!";
        form.reset();
        await loadCourses();
    } catch (error) {
        message.textContent = error.message;
    }
});

document.getElementById("refreshButton")
    .addEventListener("click", loadCourses);

loadCourses();
