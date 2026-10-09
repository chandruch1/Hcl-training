function ok() {
    let weight =parseFloat(document.getElementById("weight").value);
    let height =parseFloat( document.getElementById("Height").value);
    height=height/100;
    let BMI = weight / (height ** 2);
   
    let ans = document.querySelector("p");
     console.log(BMI);

    if (BMI < 18.5) {
        ans.innerHTML = "UnderWeight";
    }
    else if (BMI >= 18.5 && BMI <= 24.9) {
        ans.innerHTML = "Normal Weight";
    }
    else if (BMI >= 25 && BMI <= 29.9) {
        ans.innerHTML = "Over Weight";
    }
    else if (BMI >= 30 && BMI <= 34.9) {
        ans.innerHTML = "Moderate obesity";
    }
    else {
        ans.innerHTML = "Severe Obesity";
    }
}