angular.module('app.services', [])

.factory("DBConnections", ['$http', '$q', function ($http, $q) {
    return {
        all: function () {
            return $http.get('http://localhost:8080/service/multiDBService.svc/getDBConnections').then(function (response) { //wrap it inside another promise using then
                return jQuery.parseJSON(response.data);  //Se parsea a JSON
            });
        },
        remove: function(ID){ }, // TODO
    
        get: function($key, $value){ 
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);
     
            return {};
        },
    };
        }
])

.factory("dataBases", function () {
    return {

        all: function () {
            return {
                IDdb1: {
                    ID: "IDdb1",
                    name: "Base Finanzas",
                },
                IDdb2: {
                    ID: "IDdb2",
                    name: "Base Ingeniería",
                }
            }
        },
        remove: function (ID) { }, // TODO

        get: function ($key, $value) {
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
    };
})

.factory("tables", function () {
    return {
        all: function () {
            return {
                IDdb1: {
                    ID: "IDtb1",
                    name: "Usuarios",
                    alias: "DB01",
                },
                IDdb2: {
                    ID: "IDtb2",
                    name: "Puestos",
                    alias: "DB02",
                },
            }
        },
        remove: function (ID) { }, // TODO

        get: function ($key, $value) {
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
    };
})

.factory("columns", function () {
    return {
        all: function () {
            return {
                
            }
        },
        remove: function (ID) { }, // TODO

        get: function ($key, $value) {
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
    };
})


.factory("rows", function () {
    return {
        all: function () {
            return {

            }
        },
        remove: function (ID) { }, // TODO

        get: function ($key, $value) {
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
    };
})