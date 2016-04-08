angular.module('app.services', [])

.factory("DBConnections", ['$scope', '$http', '$q', function ($scope, $http, $q) {
    return {
        all: function () {
            return $http.get('http://localhost:8080/service/multiDBService.svc/getDBConnections').then(function (response) { //wrap it inside another promise using then
                return jQuery.parseJSON(response.data);  //Se parsea a JSON
            });
        },
        remove: function(params){ 
            $scope.showMessage = false;
    
            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/remove',
                method: "DELETE",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            }) 
        }, // TODO
        get: function($key, $value){ 
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);
     
            return {};
        },
        push: function (params) {
            $scope.showMessage = false;

            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/addDatabase',
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally (function () {
                $('.btn').button('reset');
            })
        }
    };
}])

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
            /*
            return $http.get('http://localhost:8080/service/multiDBService.svc/getDataBases').then(function (response) { //wrap it inside another promise using then
                return jQuery.parseJSON(response.data);  //Se parsea a JSON
            });
            */
        },
        remove: function (params) {
            $scope.showMessage = false;
    
            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/remove',
                method: "DELETE",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            }) 
        },
        get: function ($key, $value) {
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
        push: function (params) {
            $scope.showMessage = false;

            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/createDatabase',
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            })
        }
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
                /*
                return $http.get('http://localhost:8080/service/multiDBService.svc/getTables').then(function (response) { //wrap it inside another promise using then
                    return jQuery.parseJSON(response.data);  //Se parsea a JSON
                });
                */
                
            }
        },
        remove: function (params) { 
            $scope.showMessage = false;
    
            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/remove',
                method: "DELETE",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            }) 
        }, // TODO
        get: function ($key, $value) {
            console.log("DEBUG tables.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
        push: function (params) {
            $scope.showMessage = false;

            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/createTable',
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            })
        }
    };
})

.factory("columns", function () {
    return {
        all: function () {
            return $http.get('http://localhost:8080/service/multiDBService.svc/getColumns').then(function (response) { //wrap it inside another promise using then
                return jQuery.parseJSON(response.data);  //Se parsea a JSON
            });
        },
        remove: function (params) { 
            $scope.showMessage = false;
    
            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/remove',
                method: "DELETE",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            }) 
        }, // TODO
        get: function ($key, $value) {
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
        push: function (params) {
            $scope.showMessage = false;

            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/addColumn',
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            })
        }
    };
})


.factory("rows", function () {
    return {
        all: function () {
            return $http.get('http://localhost:8080/service/multiDBService.svc/getRows').then(function (response) { //wrap it inside another promise using then
                return jQuery.parseJSON(response.data);  //Se parsea a JSON
            });
        },
        remove: function (paramas) { $scope.showMessage = false;
            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/remove',
                method: "DELETE",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            }) 
        }, // TODO
        get: function ($key, $value) {
            console.log("DEBUG DBConnections.get: " + "key: " + $key + " value: " + $value);

            return {};
        },
        push: function (params) {
            $scope.showMessage = false;

            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/addRows',
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(JSON.stringify(data));
                return jQuery.parseJSON(data.data);
            })
            .error(function (data, status, headers, config) {
                $scope.message = data.error_description.replace(/["']{1}/gi, "");
                $scope.showMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            })
        }
    };
})
