/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./src/scripts/api/auth/auth.js":
/*!**************************************!*\
  !*** ./src/scripts/api/auth/auth.js ***!
  \**************************************/
/***/ (() => {

eval("const apiUrl = \"http://localhost:8081\";\r\nfunction register() {\r\n    const password = document.getElementById(\"password\").value;\r\n    const passwordRepeat = document.getElementById(\"repeat-password\").value;\r\n    if (password != passwordRepeat)\r\n        return;\r\n\r\n    const userName = document.getElementById(\"user-name\").value;\r\n    const firstName = document.getElementById(\"first-name\").value;\r\n    const secondName = document.getElementById(\"second-name\").value;\r\n    const email = document.getElementById(\"email\").value;\r\n\r\n    const userData = {\r\n        userName: userName,\r\n        firstName: firstName,\r\n        secondName: secondName,\r\n        email: email,\r\n        password: password\r\n    };\r\n\r\n    fetch(`${apiUrl}/api/auth/register`, {\r\n        method: 'POST',\r\n        headers: {\r\n            'Content-Type': 'application/json'\r\n        },\r\n        body: JSON.stringify(userData)\r\n    })\r\n        .then(response => response.json())\r\n        .then(data => {\r\n            console.log(data);\r\n            if (data.succeeded === true) {\r\n                window.location.href = 'LoginPageStudent.html';\r\n            }\r\n        })\r\n        .catch(error => {\r\n            console.error('Ошибка:', error);\r\n        });\r\n}\r\n\r\nfunction login() {\r\n    const userName = document.getElementById(\"user-name\").value;\r\n    const password = document.getElementById(\"password\").value;\r\n\r\n    const userData = {\r\n        userName: userName,\r\n        password: password\r\n    };\r\n    \r\n    fetch(`${apiUrl}/api/auth/login`, {\r\n        method: 'POST',\r\n        headers: {\r\n            'Content-Type': 'application/json'\r\n        },\r\n        body: JSON.stringify(userData)\r\n    })\r\n    .then(function(response) {\r\n        if (!response.ok) {\r\n            throw new Error(\"HTTP status \" + response.status);\r\n        }\r\n        return response.json();\r\n    })\r\n        .then(data => {\r\n            console.log(data);\r\n            localStorage.setItem('accessToken', data.accessToken);\r\n            window.location.href = '../HelloPage.html';\r\n        })\r\n        .catch(error => {\r\n            console.error(error);\r\n        });\r\n}\r\n\r\nfunction logout() {\r\n    const accessToken = localStorage.getItem(\"accessToken\");\r\n    fetch(`${apiUrl}/api/auth/logout`, {\r\n        method: 'GET',\r\n        headers: {\r\n            'Authorization': 'Bearer' + ' ' + accessToken\r\n        }\r\n    })\r\n    .then(function(response) {\r\n        if (!response.ok) {\r\n            throw new Error(\"HTTP status \" + response.status);\r\n        }\r\n        localStorage.clear();\r\n        window.location.href = '../../index.html';//todo: переделать!!!!!\r\n    })\r\n        .catch(error => {\r\n            console.error(error);\r\n        });\r\n}\r\n\r\nwindow.register = register;\r\nwindow.login = login;\r\nwindow.logout = logout;\n\n//# sourceURL=webpack://udvneolab/./src/scripts/api/auth/auth.js?");

/***/ }),

/***/ "./src/scripts/api/common/hello.js":
/*!*****************************************!*\
  !*** ./src/scripts/api/common/hello.js ***!
  \*****************************************/
/***/ (() => {

eval("const apiUrl = \"http://localhost:8081\";\r\nfunction hello() {\r\n    const accessToken = localStorage.getItem('accessToken');\r\n    const payload = parseJwt(accessToken);\r\n    const role = payload[\"http://schemas.microsoft.com/ws/2008/06/identity/claims/role\"];\r\n    const continueButton = document.getElementById('continue-btn');\r\n    if (role === \"Teacher\") {\r\n        continueButton.href = \"teacher/TeacherLabsAll.html\";\r\n    } else if (role === \"Student\") {\r\n        continueButton.href = \"student/StudentLabs.html\";\r\n    }\r\n\r\n    document.getElementById(\"role-hello\").innerText = role;\r\n    localStorage.setItem(\"currentUserId\", payload[\"userId\"]);\r\n    localStorage.setItem(\"role\", role);\r\n    fetch(`${apiUrl}/api/auth/hello`, {\r\n        method: 'GET',\r\n        headers: {\r\n            'Authorization': 'Bearer' + ' ' + accessToken\r\n        }\r\n    })\r\n        .then(function (response) {\r\n            if (!response.ok) {\r\n                throw new Error(\"HTTP status \" + response.status);\r\n            }\r\n            return response.json();\r\n        })\r\n        .then(data => {\r\n            document.getElementById(\"first-name-hello\").innerText = data.firstName;\r\n            document.getElementById(\"second-name-hello\").innerText = data.secondName;\r\n            localStorage.setItem(\"firstName\", data.firstName);\r\n            localStorage.setItem(\"secondName\", data.secondName);\r\n        })\r\n        .catch(error => {\r\n            console.error(error);\r\n        });\r\n}\r\n\r\nfunction parseJwt(token) {\r\n    var base64Url = token.split('.')[1];\r\n    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');\r\n    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {\r\n        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);\r\n    }).join(''));\r\n\r\n    return JSON.parse(jsonPayload);\r\n}\r\n\r\nwindow.hello = hello;\n\n//# sourceURL=webpack://udvneolab/./src/scripts/api/common/hello.js?");

/***/ }),

/***/ "./src/scripts/dom/userInfo.js":
/*!*************************************!*\
  !*** ./src/scripts/dom/userInfo.js ***!
  \*************************************/
/***/ (() => {

eval("function setShortUserInfo(){\r\n    const firstName = document.getElementById(\"first-name\");\r\n    firstName.innerText = localStorage.getItem(\"firstName\");\r\n    const secondName = document.getElementById(\"second-name\");\r\n    secondName.innerText = localStorage.getItem(\"secondName\");\r\n    const role= document.getElementById(\"role\");\r\n    role.innerText = localStorage.getItem(\"role\");\r\n}\r\nwindow.setShortUserInfo = setShortUserInfo;\n\n//# sourceURL=webpack://udvneolab/./src/scripts/dom/userInfo.js?");

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	__webpack_modules__["./src/scripts/api/auth/auth.js"]();
/******/ 	__webpack_modules__["./src/scripts/api/common/hello.js"]();
/******/ 	var __webpack_exports__ = {};
/******/ 	__webpack_modules__["./src/scripts/dom/userInfo.js"]();
/******/ 	
/******/ })()
;