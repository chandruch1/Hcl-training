/* let user={"age":"22"}
user['name']="chandru";
console.log(user.name);
user.name="R chandru";
console.log(user.name)
delete user.name
console.log(user.name)
console.log(user['age']) */
let halwad = false;

function inqueue() {
    return new Promise((resolve,reject)=>{
      setTimeout(()=>{
         if(halwad){
            resolve(" i baught halwa")
         }
         else{
            reject(" not halwa avaialable")
         }
      },5000)
    })
};

async function halwa(){
   
   try{
      let a= await inqueue();
      console.log(a)
   }
   catch(error){
      console.log("something fissy")
   }
   finally{
      console.log("got yo home")
   }
}
halwa();
console.log("the program is started")
