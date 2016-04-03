angular.module('app.services', [])

.factory("DBConnections", function () {
  return {
    all: function(){ 
        return {
            IDdbc1: {
                ID: "IDdbc1",
                alias: "Base Finanzas",
                user: "prueba",
                type: "Maria DB"
            },
            IDdbc2: {
                ID: "IDdbc2",
                alias: "Base Ingeniería",
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