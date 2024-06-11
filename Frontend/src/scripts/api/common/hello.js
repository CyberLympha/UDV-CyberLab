const apiUrl = process.env.AUTH_URL;
function hello() {
    const accessToken = localStorage.getItem('accessToken');
    const payload = parseJwt(accessToken);
    const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const continueButton = document.getElementById('continue-btn');
    if (role === "Teacher") {
        continueButton.href = "teacher/TeacherLabsAll.html";
    } else if (role === "Student") {
        continueButton.href = "student/StudentLabs.html";
    }

    document.getElementById("role-hello").innerText = role;
    localStorage.setItem("currentUserId", payload["userId"]);
    localStorage.setItem("role", role);
    fetch(`${apiUrl}/api/auth/hello`, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer' + ' ' + accessToken
        }
    })
        .then(function (response) {
            if (!response.ok) {
                throw new Error("HTTP status " + response.status);
            }
            return response.json();
        })
        .then(data => {
            document.getElementById("first-name-hello").innerText = data.firstName;
            document.getElementById("second-name-hello").innerText = data.secondName;
            localStorage.setItem("firstName", data.firstName);
            localStorage.setItem("secondName", data.secondName);
        })
        .catch(error => {
            console.error(error);
        });
}

function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

window.hello = hello;