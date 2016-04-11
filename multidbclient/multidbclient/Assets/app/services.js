angular.module('app.services', [])

.factory("web_services", ['$http', function ($http) {
    function $HTTPCALL(rute, method, params, $scope) {
        $scope.showSuccessMessage = false;
        $scope.showWarningMessage = false;

        $('.btn').button('loading');
        return $http({
            url: 'http://localhost:8080/service/multiDBService.svc/' + rute,
            method: method,
            headers: { 'Content-Type': 'application/json' },
            data: params
        })
        .success(function (data, status, headers, config) {
            console.log(rute + " success:");
            console.log(data);
            var data = jQuery.parseJSON(data);
            $scope.successMessage = "Rute:" + rute + " Message: " + data.message;
            $scope.showSuccessMessage = true;

            return data;
        })
        .error(function (data, status, headers, config) {
            console.log(rute + " error:");
            console.log(data);
            var data = jQuery.parseJSON(data);
            $scope.warningMessage = "Rute: " + rute + " Status: " + data.message + response.error_description.replace(/["']{1}/gi, "");
            $scope.showWarningMessage = true;

            return data
        })
        .finally(function () {
            $('.btn').button('reset');
        })
        .then(function (response) { //wrap it inside another promise using then
            console.log(rute + " then:");
            console.log(response);

            var data = jQuery.parseJSON(response.data);

            if (rute != "GET") {
                if (data.messageError) { 
                    $scope.warningMessage = "Rute:" + rute + " Error: " + data.message;
                    $scope.showWarningMessage = true;
                } else {
                    $scope.successMessage = "Rute:" + rute + " Message: " + data.message;
                    $scope.showSuccessMessage = true;
                }
                
            }

            return data;
        })
    }
    return {
        get: function (rute, params) {
            return $HTTPCALL(rute, "GET", params, {});
        },
        post: function (rute, params, $scope) {
            return $HTTPCALL(rute, "POST", params, $scope);
        },
        delete: function (rute, params, $scope) {
            return $HTTPCALL(rute, "DELETE", params, $scope);
        },
        query: function (rute, $key, $value) { // TODO
            console.log("DEBUG web_services.query: " + "key: " + $key + " value: " + $value);
            return $HTTPCALL(rute, "DELETE", params, $scope);
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
