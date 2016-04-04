angular.module('app.services', [])

.factory("DBConnections", function () {
  return {
    all: function(){ 
        return {
            IDdbc1: {
                ID: "IDdbc1",
                alias: "Conexion Maria 01",
                user: "prueba",
                type: "Maria DB"
            },
            IDdbc2: {
                ID: "IDdbc2",
                alias: "Conexion Maria 02",
                user: "prueba",
                type: "Maria DB"
            },
            IDdbc3: {
                ID: "IDdbc3",
                alias: "Conexion Mongo 01",
                user: "prueba",
                type: "Mongo DB"
            }
        }
    },
    remove: function(ID){ }, // TODO
    
    get: function($key, $value){ 
        console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);
     
        return {};
    },
  };
})

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