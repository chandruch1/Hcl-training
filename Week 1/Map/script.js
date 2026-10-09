let map = new Map();

function disp() {
    let name = document.getElementById("name").value;
    let age = document.getElementById("age").value;
    map.set(name, age);
   console.log(map)
}