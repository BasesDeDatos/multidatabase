angular.module('app.services', [])

.factory("web_services", ['$http', function ($http) {
    console.log($http)
    return {
        get: function (rute) {
            return $http.get('http://localhost:8080/service/multiDBService.svc/' + rute)
                .then(function (response) { //wrap it inside another promise using then
                    console.log(rute);
                    console.log(response.data);
                    console.log(jQuery.parseJSON(response.data));
                    return jQuery.parseJSON(response.data);  //Se parsea a JSON
                });
        },
        delete: function (rute, params, $scope) {
            $scope.successMessage = false;
            $scope.warningMessage = false;
    
            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/' + rute,
                method: "DELETE",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                $scope.successMessage = "Rute: " + rute + " Status: " + status;
                $scope.showSuccessMessage = true;
                console.log(data);
                return data;
            })
            .error(function (data, status, headers, config) {
                $scope.warningMessage = data.error_description.replace(/["']{1}/gi, "");
                $scope.showWarningMessage = true;
                console.log(JSON.stringify(data));
            })
            .finally(function () {
                $('.btn').button('reset');
            }) 
        }, 
        push: function (rute, params, $scope) {
            $scope.successMessage = false;
            $scope.warningMessage = false;

            $('.btn').button('loading');
            $http({
                url: 'http://localhost:8080/service/multiDBService.svc/' + rute,
                method: "POST",
                headers: { 'Content-Type': 'application/json' },
                data: params
            })
            .success(function (data, status, headers, config) {
                console.log(data);
                $scope.successMessage = "Rute: " + rute + " Status: " + status;
                $scope.showSuccessMessage = true;
                return data.data;
            })
            .error(function (data, status, headers, config) {
                $scope.warningMessage = "Rute: " + rute + " Error: "+ data.error_description.replace(/["']{1}/gi, "");
                $scope.showWarningMessage = true;
                console.log(data);
            })
            .finally (function () {
                $('.btn').button('reset');
            })
        },
        query: function (rute, $key, $value) { // TODO
            console.log("DEBUG web_services.query: " + "key: " + $key + " value: " + $value);
            return {};
        },
    };
}])

.factory("dataBases", ['$http', function ($http)  {
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
}])

.factory("tables", ['$http', function ($http)  {
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
}])

.factory("columns", ['$http', function ($http) {
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
}])

.factory("rows", ['$http', function ($http) {
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
}])
