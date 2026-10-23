
const API_URL = "http://localhost:5000/api/questions";

const form = document.getElementById("questionForm");
const table = document.getElementById("questionTable");
const message = document.getElementById("message");
const loading = document.getElementById("loading");

const searchInput = document.getElementById("searchId");
const searchButton = document.getElementById("searchBtn");
const showAllButton = document.getElementById("showAllBtn");
const searchMessage = document.getElementById("searchMessage");
const searchTable = document.getElementById("searchTable");
const searchResult = document.getElementById("searchResult");

// Show success or error message
function showMessage(element, text, isError = false) {
    element.textContent = text;
    element.style.color = isError ? "red" : "green";
}

// Common function for HTTP requests
async function apiRequest(url, options = {}) {
    const response = await fetch(url, options);
    const text = await response.text();

    if (!response.ok) {
        throw new Error(
            text || `Request failed: HTTP ${response.status}`
        );
    }

    if (!text) return null;

    try {
        return JSON.parse(text);
    } catch {
        return text;
    }
}

// Read the form values
function getFormData() {
    return {
        subject: document.getElementById("subject").value.trim(),
        text: document.getElementById("questionText").value.trim(),
        optionA: document.getElementById("optionA").value.trim(),
        optionB: document.getElementById("optionB").value.trim(),
        optionC: document.getElementById("optionC").value.trim(),
        optionD: document.getElementById("optionD").value.trim(),
        correctOption: document.getElementById("correctAnswer").value
    };
}

// Get the question ID from the API response
function getQuestionId(q) {
    return q.id ?? q.questionId ?? q.Id ?? q.QuestionId;
}

// Display options as text
function getOptions(q) {
    return `A: ${q.optionA ?? q.OptionA ?? ""}\n` +
           `B: ${q.optionB ?? q.OptionB ?? ""}\n` +
           `C: ${q.optionC ?? q.OptionC ?? ""}\n` +
           `D: ${q.optionD ?? q.OptionD ?? ""}`;
}

// Create a table row for a question
function createQuestionRow(q) {
    const row = document.createElement("tr");
    const id = getQuestionId(q);

    const values = [
        id,
        q.subject ?? q.Subject,
        q.text ?? q.Text,
        getOptions(q),
        q.correctOption ?? q.CorrectOption
    ];

    values.forEach(value => {
        const cell = document.createElement("td");
        cell.textContent = value ?? "";
        row.appendChild(cell);
    });

    const actions = document.createElement("td");

    const editButton = document.createElement("button");
    editButton.textContent = "Edit";
    editButton.className = "edit-btn";
    editButton.addEventListener("click", () => editQuestion(id));

    const deleteButton = document.createElement("button");
    deleteButton.textContent = "Delete";
    deleteButton.className = "delete-btn";
    deleteButton.addEventListener("click", () => deleteQuestion(id));

    actions.append(editButton, deleteButton);
    row.appendChild(actions);

    return row;
}

// GET: Load all questions
async function loadQuestions() {
    loading.hidden = false;
    loading.textContent = "Loading questions...";
    table.replaceChildren();

    try {
        const data = await apiRequest(API_URL);

        const questions = Array.isArray(data)
            ? data
            : data?.$values;

        if (!Array.isArray(questions)) {
            throw new Error("API did not return a question list.");
        }

        questions.forEach(q => {
            table.appendChild(createQuestionRow(q));
        });

        loading.hidden = questions.length > 0;
        loading.textContent = "No questions found.";

    } catch (error) {
        loading.hidden = false;
        loading.textContent = "Failed to load questions.";
        showMessage(message, error.message, true);
    }
}

// Search: GET /api/questions/{id}
async function searchQuestion() {
    const id = searchInput.value.trim();

    if (!id || !Number.isInteger(Number(id)) || Number(id) <= 0) {
        searchTable.hidden = true;
        searchResult.replaceChildren();
        showMessage(searchMessage, "Please enter a valid question ID.", true);
        return;
    }

    searchButton.disabled = true;
    showMessage(searchMessage, "Searching...");
    searchTable.hidden = true;
    searchResult.replaceChildren();

    try {
        const q = await apiRequest(
            `${API_URL}/${encodeURIComponent(id)}`
        );

        if (!q || typeof q !== "object" || Array.isArray(q)) {
            throw new Error("Question not found.");
        }

        searchResult.appendChild(createQuestionRow(q));
        searchTable.hidden = false;

        showMessage(searchMessage, `Question ${id} found.`);

    } catch (error) {
        showMessage(
            searchMessage,
            error.message.includes("404")
                ? `Question ID ${id} was not found.`
                : `Search failed: ${error.message}`,
            true
        );
    } finally {
        searchButton.disabled = false;
    }
}

// Search button click
searchButton.addEventListener("click", searchQuestion);
   
// Press Enter to search
searchInput.addEventListener("keydown", event => {
    if (event.key === "Enter") {
        event.preventDefault();
        searchQuestion();
    }
});

// Show All: reload the complete list
showAllButton.addEventListener("click", async () => {
    searchInput.value = "";
    searchResult.replaceChildren();
    searchTable.hidden = true;
    showMessage(searchMessage, "");
    await loadQuestions();

    document.getElementById("allQuestionsSection")
        .scrollIntoView({ behavior: "smooth" });
});

// POST: Add / PUT: Update
form.addEventListener("submit", async event => {
    event.preventDefault();

    const id = document.getElementById("id").value;
    const editing = id !== "";
    const saveButton = document.getElementById("saveBtn");

    saveButton.disabled = true;

    try {
        await apiRequest(
            editing ? `${API_URL}/${encodeURIComponent(id)}` : API_URL,
            {
                method: editing ? "PUT" : "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(getFormData())
            }
        );

        showMessage(
            message,
            editing
                ? "Question updated successfully!"
                : "Question added successfully!"
        );

        resetForm();
        searchResult.replaceChildren();
        searchTable.hidden = true;
        await loadQuestions();

    } catch (error) {
        showMessage(message, error.message, true);
    } finally {
        saveButton.disabled = false;
    }
});

// GET by ID: Load question into form for editing
async function editQuestion(id) {
    try {
        const q = await apiRequest(
            `${API_URL}/${encodeURIComponent(id)}`
        );

        if (!q || typeof q !== "object") {
            throw new Error("Question not found.");
        }

        document.getElementById("id").value =
            getQuestionId(q) ?? id;

        document.getElementById("subject").value =
            q.subject ?? q.Subject ?? "";

        document.getElementById("questionText").value =
            q.text ?? q.Text ?? "";

        document.getElementById("optionA").value =
            q.optionA ?? q.OptionA ?? "";

        document.getElementById("optionB").value =
            q.optionB ?? q.OptionB ?? "";

        document.getElementById("optionC").value =
            q.optionC ?? q.OptionC ?? "";

        document.getElementById("optionD").value =
            q.optionD ?? q.OptionD ?? "";

        document.getElementById("correctAnswer").value =
            q.correctOption ?? q.CorrectOption ?? "";

        document.getElementById("formTitle").textContent = "Edit Question";
        document.getElementById("saveBtn").textContent = "Update Question";
        document.getElementById("cancelBtn").hidden = false;

        showMessage(message, `Editing question ${id}.`);

        window.scrollTo({ top: 0, behavior: "smooth" });

    } catch (error) {
        showMessage(message, error.message, true);
    }
}

// DELETE: Remove a question
async function deleteQuestion(id) {
    if (!confirm(`Are you sure you want to delete question ${id}?`)) {
        return;
    }

    try {
        await apiRequest(
            `${API_URL}/${encodeURIComponent(id)}`,
            { method: "DELETE" }
        );

        showMessage(message, "Question deleted successfully!");

        resetForm();
        searchResult.replaceChildren();
        searchTable.hidden = true;

        await loadQuestions();

    } catch (error) {
        showMessage(message, error.message, true);
    }
}

// Reset form
function resetForm() {
    form.reset();

    document.getElementById("id").value = "";
    document.getElementById("formTitle").textContent = "Add Question";
    document.getElementById("saveBtn").textContent = "Add Question";
    document.getElementById("cancelBtn").hidden = true;
}

// Cancel editing
document.getElementById("cancelBtn")
    .addEventListener("click", resetForm);

// Refresh all questions
document.getElementById("refreshBtn")
    .addEventListener("click", loadQuestions);

// Initial load
loadQuestions();
