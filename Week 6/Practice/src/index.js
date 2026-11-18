"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
let students = [
    {
        id: 101,
        Name: "Chandru",
        Age: 21,
        dept: "CSBS",
        email: "chandru@gmail.com"
    },
    {
        id: "CS102",
        Name: "Arun",
        Age: 22,
        dept: "CSE"
    }
];
function displayStudentDetails(students) {
    students.forEach(element => {
        console.log(`The ID od the Student : ${element.id}`);
        console.log(`The Name Of the Student : ${element.Name}`);
        console.log(`The Age of the Student : ${element.Age}`);
    });
}
let studentcont = [{
        Name: "chandru",
        Age: 21,
        id: 4,
        dept: "csbs",
        Ph: 8838278675,
        compamy: "Abc infotech",
        address: "dindigul"
    }
];
studentcont.push({
    Name: "harin",
    Age: 21,
    id: 4,
    dept: "csbs",
    Ph: 8838278675,
    compamy: "Abc infotech",
    address: "dindigul"
});
console.log(studentcont[1]);
