function bill() {
    let milk = Number(document.getElementById("milk").value);
    let lays = Number(document.getElementById("lays").value);
    let bingo = Number(document.getElementById("bingo").value);
    let milkshake = Number(document.getElementById("milkshake").value);

    let milkPrice = 30;
    let laysPrice = 20;
    let bingoPrice = 20;
    let milkshakePrice = 50;

    let milkAmount = milk * milkPrice;
    let laysAmount = lays * laysPrice;
    let bingoAmount = bingo * bingoPrice;
    let milkshakeAmount = milkshake * milkshakePrice;

    let total = milkAmount + laysAmount + bingoAmount + milkshakeAmount;

    console.log(total)

    let printbutton = document.querySelector(".printbutton");

    printbutton.innerHTML = `
        <h1>Payment Bill</h1>
        <p>Milk: ${milk} × ₹${milkPrice} = ₹${milkAmount}</p>

        <p>Lays: ${lays} × ₹${laysPrice} = ₹${laysAmount}</p>

        <p>Bingo: ${bingo} × ₹${bingoPrice} = ₹${bingoAmount}</p>

        <p>MilkShake: ${milkshake} × ₹${milkshakePrice} = ₹${milkshakeAmount}</p>

        <hr>

        <h3>Total: ₹${total}</h3>
    `;
    printbutton.style.display = "block";
}