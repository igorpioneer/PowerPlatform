window.onload = () =>{
    let url = window.location.href;

    if(url.indexOf("campaign-customer") == -1){
        document.querySelector("#btnLogin").addEventListener("click", function(){
            let username, password, errors
        
            username = document.querySelector("#tbUsername")
            password = document.querySelector("#tbPassword")
            errors = 0
            
            if(username.value == ""){
                errors ++
            }
            if(password.value == ""){
                errors ++
            }
            console.log(errors)

            if(errors == 0){
                loginFunction(username, password)
            }
            else {
                displayMessage("Your login credentials are incorrect.", "error")
            }
        })
    }
    else{
        let customerId, campaignId
        
        customerId = document.querySelector("#tbCustomerId")
        campaignId = document.querySelector("#ddlCampaign")

        populateDdlCampaign();

        document.querySelector("#btnInsert").addEventListener("click", function(){
            fetch('http://localhost:5000/api/CampaignCustomer', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                },
                body: JSON.stringify({
                    customerId: customerId.value,
                    campaignId: campaignId.value
                })
                })
                .then(response => response.json())
                .then(data => {
                if(response.status == "200")
                    console.log("Success!")
                })
                .catch(error => {
                console.error("Error inserting data:", error.message);
                });
        })
    }
}

function populateDdlCampaign() {
    let host = "http://" + window.location.host;

    fetch('http://localhost:5000/api/CampaignCustomer', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        },
    })
        .then(response => {
            if (!response.ok) {
                displayMessage("Your token has expired. You need to log in again.", "error");
                window.setTimeout(function () {
                    window.location.replace(host + "/Frontend/")
                }, 2000)
            }
            return response.json();
        })
        .then(data => {
            printDropDownList(data);
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error);
        });
}

function loginFunction(username, password) {

    let host = "http://" + window.location.host;

    fetch('http://localhost:5000/api/Token', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: username.value,
            password: password.value
        })
    })
        .then(response => {
            if (!response.ok) {
                displayMessage("Status: " + response.status + "; " + response.statusText, "error")
            }
            return response.json()
        })
        .then(data => {
            localStorage.setItem('token', data.token)
        })
        .catch(error => {
            displayMessage("There was a problem with local storage. Try to refresh your web browser.", "error")
        })


    fetch('http://localhost:5000/api/Token', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + localStorage.getItem('token')
        },
        body: JSON.stringify({
            username: username.value,
            password: password.value
        })
        })
        .then(response => {
            if (!response.ok) {
                displayMessage(response.status + "; " + response.statusText + ". Enter your credentials carefully!", "error")
            }
            return response.json()
        })
        .then(data => {
            displayMessage("You have successfully logged in!", "success")
            window.setTimeout(function () {
                window.location.replace(host + "/Frontend/campaign-customer.html")
            }, 1000)
        })
        .catch(error => {
            console.error('There was a problem with the fetch operation:', error)
        })
}

function displayMessage(message, type){
    let html = ""
    let successClass = "success"
    let errorClass = "danger"

    if(type == "success"){
        html += `<p class="alert alert-${successClass} text-center">${message}</p>`
    }
    else {
        html += `<p class="alert alert-${errorClass} text-center">${message}</p>`
    }
    document.querySelector(".inner-html-response").innerHTML = html;
}

function printDropDownList(data){
    let html = ""
    data.forEach(campaign => {
        html += `<option value="${campaign.campaignId}">${campaign.name}</option>`
    });
    document.querySelector("#ddlCampaign").innerHTML = html;
}