/*
\file:      employee
\author:    Vincent Li
\purpose:   This is the js file for the employee html page
*/  

$(function () {
    // validation for lab 12
    // takes every keystroke and clears existing css classes on status div
    // once the user stops typing, it will automatically check if valid
    // needs a form to work
    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass(); // remove any existing css on div
        if ($("#EmployeeModalForm").valid()) {
            $("#modalstatus").attr("class", "badge badge-success"); // green badge will appear for valid data
            $("#modalstatus").text("data entered is valid");
            $("#actionbutton").prop('disabled', false); // enable action button
        }
        else {
            $("#modalstatus").attr("class", "badge badge-danger"); // red badge will appear for invalid
            $("#modalstatus").text("fix errors");
            $("#actionbutton").prop('disabled', true); // disable action button
        }
    });

    // the entire method contents is a JSON, includes 3 properties of rules, errorElement, and messages
    // validTitle is a custom rule
    // maxlength and required are standard rules
    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstname: { maxlength: 25, required: true },
            TextBoxLastname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: "div",
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need valid email format"
            }
        }
    }); // EmployeeModalForm.validate

    $.validator.addMethod("validTitle", (value) => { // custom rule
        return (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
    }, ""); // .validator.addMethod

    // get all
    const getAll = async (msg) => {
        try {
            $(`#employeeList`).text(`Finding employee Information...`);
            let response = await fetch(`api/employee`);
            if (response.ok) {
                let data = await response.json(); // this returns a promise so we await it
                buildemployeeList(data);// builds ppl
                loadDepartmentDDL(); // loads departments

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

    // setting up the update
    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update employee</h4>");
        $("#theModal").modal("toggle");

        // clear the field
        $("#uploader").val("");

        data.map(employee => {
            if (employee.id === parseInt(id)) {
                $("#theModal").modal("toggle");
                $("#deletebutton").show();

                // gets info to update
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstname").val(employee.firstname);
                $("#TextBoxLastname").val(employee.lastname);
                $("#TextBoxPhone").val(employee.phoneno);
                $("#TextBoxEmail").val(employee.email);

                // picture part
                //$("#ImageHolder").html(`<img height="120" width="110" src="data:img/png;base64,${employee.staffPicture64}" />`);
                $("#ImageHolder").html(`<img height="120" width="110" src="data:img/png;base64,${employee.picture64}" />`);

                // set value for ddl department
                $('#ddlDepartments').val(employee.departmentId);

                // option a, either store the data columns separately
                sessionStorage.setItem("id", employee.id); 
                sessionStorage.setItem("departmentId", employee.departmentId);
                sessionStorage.setItem("timer", employee.timer);
                //sessionStorage.setItem("picture", employee.staffPicture64);
                sessionStorage.setItem("picture", employee.picture64);

                // option b, or store them as one person
                sessionStorage.setItem('selectedEmployee', JSON.stringify(employee));
                $("#modalstatus").text("update data");

                // checking for valid data
                let validator = $("#EmployeeModalForm").validate();
                validator.resetForm();
                $("#modalstatus").attr("class", "");
            } // if
        }); //datamap
    }; //setup for update

    // clears fields
    const clearModalFields = () => {
        // clears boxes and session storage
        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");
        $("#uploader").val("");
        $("#ImageHolder").html(`<img height="120" width="110" src="data:img/png;base64" />`); // reset image
        sessionStorage.removeItem("id");
        sessionStorage.removeItem("departmentId"); // might be upper case like departmentId
        sessionStorage.removeItem("timer");
        sessionStorage.removeItem("picture"); // might need to change the html picture to nothing as well

        // removes employee selected and resets department
        sessionStorage.removeItem('selectedEmployee');
        loadDepartmentDDL(-1);

        // validation
        $("#EmployeeModalForm").validate().resetForm();
    }; // clear modal fields

    // loading departments in drop down list
    let loadDepartmentDDL = async () => {
        // set all divs to local storage 
        response = await fetch('api/department');
        if (!response.ok)
            throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
        let divs = await response.json();

        html = '';
        $('#ddlDepartments').empty();
        // add default value
        html += `<option value="" disabled selected>Choose your option</option>`
        divs.map(div =>
            html += `<option value="${div.id}">${div.name}</option>`
        );
        $('#ddlDepartments').append(html);
    }

    // setting up the add
    const setupForAdd = () => {
        clearModalFields();
        $('#ddlDepartments').val(-1); // clearing info

        sessionStorage.setItem("picture", $('#uploader').val());

        $("#actionbutton").val("add");
        $("#deletebutton").hide();
        $("#modaltitle").html("<h4>add employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new employee");
    }; //setupforadd


    // add method
    const add = async () => {
        try {
            stu = new Object();

            // id set by database but need to be set bc server cannot accept nulls
            stu.id = -1;

            // pull div id from ddl
            stu.departmentId = parseInt($('#ddlDepartments').val());

            // pull the rest of the data
            stu.title = $("#TextBoxTitle").val();
            stu.firstname = $("#TextBoxFirstname").val();
            stu.lastname = $("#TextBoxLastname").val();
            stu.phoneno = $("#TextBoxPhone").val();
            stu.email = $("#TextBoxEmail").val();

            sessionStorage.getItem("picture") ? stu.picture64 = sessionStorage.getItem("picture") : stu.picture64 = null;
            
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
            $("#theModal").modal("toggle");
        } catch (exception) {
            $("#status").text(error.message);
        } // try/catch
    }; // add

    // update function
    const update = async () => {
        try {

            // set up a new cleint side instance of employee
            stu = JSON.parse(sessionStorage.getItem('selectedEmployee'));

            stu.departmentId = parseInt($('#ddlDepartments').val());

            // populate properties
            stu.title = $("#TextBoxTitle").val();
            stu.firstname = $("#TextBoxFirstname").val();
            stu.lastname = $("#TextBoxLastname").val();
            stu.phoneno = $("#TextBoxPhone").val();
            stu.email = $("#TextBoxEmail").val();

            //setting image
            //sessionStorage.getItem("picture") ? stu.staffPicture64 = sessionStorage.getItem("picture") : stu.staffPicture64 = null;
            sessionStorage.getItem("picture") ? stu.picture64 = sessionStorage.getItem("picture") : stu.picture64 = null;

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

    //aciton button for either add or update
    $("#actionbutton").click(() => {
        $("#actionbutton").val() === "update" ? update() : add();
    });


    // builds employeelist, modified to work with search
    const buildemployeeList = (data, usealldata = true) => {
        //sessionStorage.setItem("allemployees", JSON.stringify(data)); // storing employee data to session storage

        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">employee Info</div>
                  <div class="list-group-item row d-flex text-center" id="heading">
                  <div class="col-4 h4">Title</div>
                  <div class="col-4 h4">First</div>
                  <div class="col-4 h4">Last</div>
                </div>`);
        div.appendTo($("#employeeList"));
        usealldata ? sessionStorage.setItem("allemployees", JSON.stringify(data)) : null; // storing employee data to session storage
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

    // when clicking on list
    $(`#employeeList`).click((e) => {

        // Keep track of where they clicked
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === `employeeList` || id === ``) {
            id = e.target.id;
        } // clicked on row somewhere else

        // making sure they clicked on the list and not the heading or status
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allemployees")); // currently have the employee id, now it is going to look at all the employees from the session storage
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they click on heading or status
        }
    }); // employeelist click

    // jquery finds data then gets confirmation
    // look at bootstrap js library for delete confirmation
    // Basically, it needs to be the same to delete properly
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
    // more confirmation stuff
    $('#deletebutton').click(() => _delete()); // if yes was chosen

    // delete function
    const _delete = async () => {
        try {
            let response = await fetch(`api/employee/${sessionStorage.getItem('id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for reponse.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server side console`);
            }
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }; // delete

    // Search
    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allemployees"));
        let filtereddata = alldata.filter((stu) => stu.lastname.match(new RegExp($("#srch").val(), 'i')));
        buildemployeeList(filtereddata, false);
    }); // srch keyup

    // do we have a picture?
    $("input:file").change(() => {
        const reader = new FileReader();
        const file = $("#uploader")[0].files[0];

        file ? reader.readAsBinaryString(file) : null;

        reader.onload = (readerEvt) => {
            // get binary data then convert to encoded string
            const binaryString = reader.result;
            const encodedString = btoa(binaryString);
            sessionStorage.setItem('picture', encodedString)
        };
    }); // input file change

    getAll(""); // first grab the data from the server
    loadDepartmentDDL(); // since it is static info, might as well only load it once
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