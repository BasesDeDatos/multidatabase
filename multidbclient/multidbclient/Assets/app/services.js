angular.module('app.services', [])

.factory("DBConnections", function ($http, $q) {
    
    return {
        all: function(){ 

            var temp = {};
            var defer = $q.defer();

            /*$http.get('http://localhost:8080/service/multiDBService.svc/getDBConnections').success(function (data) {
                alert(data);
                temp = data;
                defer.resolve(data);
            });*/
            return {}//}//defer.promise;            
        },
        remove: function(ID){ }, // TODO
    
        get: function($key, $value){ 
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);
     
            return {};
        },
    };
        }
)

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