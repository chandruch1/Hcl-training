let obj=[];
function add(){
  let Name = document.getElementById("name").value;
    let para = document.getElementById("para");
    if(Name==""){
      para.innerHTML=` <h1>please insert Name </h1>`;
      return;
    }
    obj.push(Name);

    para.innerHTML = `<h1>${Name} is added Successfully</h1>`;

    console.log(obj);
  
}
function del(){
  let ans=obj.pop();
  let para=document.getElementById("para");
  if(ans==undefined){
    para.innerHTML = `<h1>No name is exist</h1>`;
  }
  else{
  para.innerHTML = `<h1>${ans} is delete Successfully</h1>`;
  }
  console.log(obj)
}
function view(){
  let line=document.getElementById("line");
  para.innerHTML = ""; 
  line.querySelectorAll("h3").forEach(h3 => h3.remove());
  for(let i=0;i<obj.length;i++){
    let h3=document.createElement("h3");
    h3.innerHTML=`${obj[i]}`;
    line.appendChild(h3);
    console.log(obj[i]);
  }
}

