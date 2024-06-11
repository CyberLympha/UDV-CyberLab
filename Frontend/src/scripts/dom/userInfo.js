function setShortUserInfo(){
    const firstName = document.getElementById("first-name");
    firstName.innerText = localStorage.getItem("firstName");
    const secondName = document.getElementById("second-name");
    secondName.innerText = localStorage.getItem("secondName");
    const role= document.getElementById("role");
    role.innerText = localStorage.getItem("role");
}
window.setShortUserInfo = setShortUserInfo;