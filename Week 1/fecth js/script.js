async function fun() {
    let response = await fetch('info.txt');
    let txt = await response.text();

    let para = document.getElementById("para");
    para.innerText = txt;
    console.log(txt);
}

fun();