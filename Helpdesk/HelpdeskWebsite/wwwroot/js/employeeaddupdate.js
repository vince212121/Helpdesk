/*
\file:      employeeaddupdate
\author:    Vincent Li
\purpose:   This file is used for the add and update part for the employee
*/

$(function () {
    const getAll = async (msg) => {
        try {
            $(`#employeeList`).text(`Finding employee Information...`);
            let response = await fetch(`api/employee`);
            if (response.ok) {
                let data = await response.json(); // this returns a promise so we await it
                buildemployeeList(data);
                msg === `` ? // are we appending to an existing message
                    $("#status").text("employees Loaded") : $("#status").text(`${msg}`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // 404 not found 
                $("#status").text("no such path on server");
            }// else
        } catch (error) {
            $("#status").text(error.message);
        }
    }; // getall

    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update employee</h4>");

        clearModalFields();
        data.map(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstname").val(employee.firstname);
                $("#TextBoxLastname").val(employee.lastname);
                $("#TextBoxPhone").val(employee.phoneno);
                $("#TextBoxEmail").val(employee.email);
                sessionStorage.setItem("id", employee.id);
                sessionStorage.setItem("departmentId", employee.departmentId);
                sessionStorage.setItem("timer", employee.timer);
                $("#modalstatus").text("update data");
                $("#theModal").modal("toggle");
            } // if
        }); //datamap
    }; //setup for update

    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new employee");
    }; //setupforadd

    const clearModalFields = () => {
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("departmentId"); // might be upper case like departmentId
        sessionStorage.removeItem("timer");
    }; // clear modal fields

    const add = async () => {
        try {
            stu = new Object();
            stu.title = $("#TextBoxTitle").val();
            stu.firstname = $("#TextBoxFirstname").val();
            stu.lastname = $("#TextBoxLastname").val();
            stu.phoneno = $("#TextBoxPhone").val();
            stu.email = $("#TextBoxEmail").val();
            stu.departmentName = null;
            stu.staffpicture64 = null;

            stu.departmentId = 100; // hard code for now, we will add a drop down later
            stu.id = -1;
            stu.timer = null;

            // send the employee info to the server asynchronously using POST
            let response = await fetch(`api/employee`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(stu)
            });
            if (response.ok) // or check for response status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // prob some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            } // else
        } catch (exception) {
            $("#status").text(error.message);
        } // try/catch
        $("#theModal").modal("toggle");
    }; // add

    const update = async () => {
        try {
            // set up a new cleint side instance of employee
            stu = new Object();
            // populate properties
            stu.title = $("#TextBoxTitle").val();
            stu.firstname = $("#TextBoxFirstname").val();
            stu.lastname = $("#TextBoxLastname").val();
            stu.phoneno = $("#TextBoxPhone").val();
            stu.email = $("#TextBoxEmail").val();
            stu.departmentName = null;
            stu.staffpicture64 = null;

            // take the values from earlier
            // numbered needed for Ids or http 401
            stu.id = parseInt(sessionStorage.getItem("id")); // local storage is something the browser uses and it doesn't delete if browser is closed
            stu.departmentId = parseInt(sessionStorage.getItem("departmentId")); // session storage loses its information once the website/browser is closed
            stu.timer = sessionStorage.getItem("timer"); // get item takes 1 parameter which is the name stored

            // send the updated back to the server asynchronously using PUT
            let response = await fetch("api/employee", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(stu)
            });

            if (response.ok) // or check for response status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            }// else

        } catch (error) {
            $("#status").text(error.message);
            console.table(error);
        } // try/catch
        $("#theModal").modal("toggle");

    }; // update

    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    });

    const buildemployeeList = (data) => {
        sessionStorage.setItem("allemployees", JSON.stringify(data)); // storing employee data to session storage

        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">employee Info</div>
                  <div class="list-group-item row d-flex text-center" id="heading">
                  <div class="col-4 h4">Title</div>
                  <div class="col-4 h4">First</div>
                  <div class="col-4 h4">Last</div>
                </div>`);
        div.appendTo($("#employeeList"));
        btn = $(`<button class="list-group-item row d-flex" id="0"><div class="col-12 text-left">...click to add employee</div></button>`);
        btn.appendTo($("#employeeList"));
        data.map(stu => {
            btn = $(`<button class="list-group-item row d-flex" id="${stu.id}">`);
            btn.html(`<div class="col-4" id ="employeetitle${stu.id}">${stu.title}</div>
                      <div class="col-4" id ="employeefname${stu.id}">${stu.firstname}</div>
                      <div class="col-4" id ="employeelname${stu.id}">${stu.lastname}</div>` // note in the pdf, the lastname is spelt like employeelastnam
            );
            btn.appendTo($("#employeeList"));
        }); // map
    } // build employee list



    $(`#employeeList`).click((e) => {

        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === `employeeList` || id === ``) {
            id = e.target.id;
        } // clicked on row somewhere else

        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allemployees")); // currently have the employee id, now it is going to look at all the employees from the session storage
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they click on heading or status
        }
    }); // employeelist click



    getAll(""); // first grab the data from the server
}); // jQuery ready method

// server was reached out but server had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // else
}