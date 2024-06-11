const accessToken = localStorage.getItem("accessToken");
const apiUrl = process.env.LABS_URL;

function getAllTeacherLabs() {
    const url = `${apiUrl}/Labs/teacher/labs`;

    fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer' + ' ' + accessToken
        }
    })
        .then(response => {
            if (!response.ok) {
                if (response.status == 401)
                    window.location.href = "../auth/LoginPageStudent.html"
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            addLabCardsToDOM(data);
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
}

function addLabCardsToDOM(labs) {
    const labsList = document.querySelector('.labs-list');
    labs.forEach(lab => {
        const labCard = document.createElement('a');
        labCard.classList.add('lab-card-mini');
        labCard.href = 'TeacherLabs.html';
        labCard.id = `${lab.id}`;
        labCard.onclick = function(e){
            localStorage.setItem("currentLabId", lab.id);
            localStorage.setItem("currentLabName", lab.name);
        }

        const labName = document.createElement('span');
        labName.classList.add('heading', 'lab-name');
        labName.textContent = lab.name;

        labCard.appendChild(labName);
        labsList.appendChild(labCard);
    });
}

function getUserLabsPerLab(labId){
    const url = `${apiUrl}/Labs/${labId}/userLabs`;
    fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer' + ' ' + accessToken
        }
    })
        .then(response => {
            if (!response.ok) {
                if (response.status == 401)
                    window.location.href = "../auth/LoginPageStudent.html"
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            addUserLabCardsToDOM(data);
        })
        .catch(error => {
            console.error('There has been a problem with your fetch operation:', error);
        });
}

function addUserLabCardsToDOM(labs) {
    const labsList = document.querySelector('.labs-list');
    labs.forEach(lab => {
        const labCard = document.createElement('a');
        labCard.classList.add('lab-card-mini');
        labCard.href = 'TeacherLab.html';
        labCard.onclick = function(e){
            localStorage.setItem("currentUserLabId", lab.id);
        }

        const heading = document.createElement('span');
        heading.classList.add('heading');

        const firstName = document.createElement('span');
        firstName.id = "firstName";
        firstName.textContent = lab.firstName;

        const secondName = document.createElement('span');
        secondName.id = "secondName";
        secondName.textContent = lab.secondName;

        heading.appendChild(firstName);
        heading.appendChild(document.createTextNode(' '));
        heading.appendChild(secondName);

        const lastSentAt = document.createElement('span');
        lastSentAt.id = "lastSentAt";
        lastSentAt.textContent = formatDate(lab.lastSentAt);

        const lastRate = document.createElement('span');
        lastRate.id = "lastRate";
        lastRate.textContent = `${lab.lastRate}/100 баллов`;

        labCard.appendChild(heading);
        labCard.appendChild(lastSentAt);
        labCard.appendChild(lastRate);

        labsList.appendChild(labCard);
    });
}

function formatDate(dateString) {
    const options = {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    };
    return new Date(dateString).toLocaleString('ru-RU', options).replace(',', ' ');
}

function getAttemptInfo(){
    const url = `${apiUrl}/Labs/attempts/${localStorage.getItem("currentUserLabId")}`;
    fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer' + ' ' + accessToken
        }
    })
        .then(response => {
            if (!response.ok) {
                if (response.status == 401)
                    window.location.href = "../auth/LoginPageStudent.html"
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            setAttemptContent(data);
        })
        .catch(error => {
            console.error('Error fetching data:', error);
        });
} 

function updateUserLabRate() {
    const newRate = Number(document.getElementById('new-rate').value);
    const url = `${apiUrl}/Labs/attempts/${localStorage.getItem("currentUserLabId")}`;
    const payload = { newRate: newRate };

    fetch(url, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer' + ' ' + accessToken
        },
        body: JSON.stringify(payload)
    })
        .then(response => {
            if (!response.ok) {
                if (response.status == 401)
                    window.location.href = "../auth/LoginPageStudent.html"
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            setAttemptContent(data);
        })
        .catch(error => {
            console.error('Error saving rate:', error);
        });
}

function setAttemptContent(data) {
    document.getElementById('second-name-student').textContent = data.secondName;
    document.getElementById('first-name-student').textContent = data.firstName;
    document.getElementById('last-sent-at').textContent = formatDate(data.lastSentAt);
    document.getElementById('last-rate').textContent = `${data.lastRate}/100 баллов`;
    document.getElementById('report').textContent = data.report;
}

window.getAllTeacherLabs = getAllTeacherLabs;
window.getUserLabsPerLab = getUserLabsPerLab;
window.getAttemptInfo = getAttemptInfo;
window.updateUserLabRate = updateUserLabRate;