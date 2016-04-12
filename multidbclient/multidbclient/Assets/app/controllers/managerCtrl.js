﻿angular.module('manager', [])

.controller('managerCtrl', [
    '$scope',
    '$http',
    'web_services',
    function ($scope, $http, web_services) {

        $scope.DBConnections = false; //se inicia en false para que muestre el loader en la página
        $scope.databases = false;
        $scope.tables = false;
        $scope.columns = false;

        web_services.get('getDBConnections').then(function (response) { //Async call to DBConnections factory
            $scope.DBConnections = response; //Assign data received to $scope.data
        });

        web_services.get('getDatabases').then(function (response) { //Async call to DBConnections factory
            $scope.databases = response; //Assign data received to $scope.data
        });

        web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
            $scope.tables = response; //Assign data received to $scope.data
        });

        web_services.get('getColumns').then(function (response) { //Async call to DBConnections factory
            $scope.columns = response; //Assign data received to $scope.data
        });

        $scope.addDatabase = function () {
            var params = {
                database_type: $scope.database_type,
                user: $scope.user,
                pass: $scope.pass,
                server: $scope.server,
                protocol: $scope.protocol,
                port: $scope.protocol,
                alias: $scope.alias
            };
            web_services.post("addDatabase", params, $scope).finally(function () {
                web_services.get('getDBConnections').then(function (response) { //Async call to DBConnections factory
                    $scope.DBConnections = response; //Assign data received to $scope.data
                });
            });
        }

        $scope.createDatabase = function () {
            var params = { name: $scope.DB_name };
            web_services.post("createDatabase", params, $scope).finally(function () {
                web_services.get('getDatabases').then(function (response) { //Async call to DBConnections factory
                    $scope.databases = response; //Assign data received to $scope.data
                });
            });
                
        }
            
        $scope.createTable = function () {
            var params = {
                table_name: $scope.table_name,
                database_id: $scope.database_id,
                columns: [],
            }
            $("table tbody tr").each(function () {
                $row = $(this);
                params.columns.push({
                    DB_alias: $row.find(".table_alias").val(),
                    column_name: $row.find(".table_name").val(),
                    Type: $row.find(".table_type").val(),
                    Null: $row.find(".table_null").val()
                })
            })
            web_services.post("createTable", params, $scope).finally(function () {
                web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
                    $scope.tables = response; //Assign data received to $scope.data
                });
            });
                
        }

        $scope.dropTable = function () {
            web_services.delete('dropTable', { ID: $scope.drop_table_ID }, $scope).then(function () {
                web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
                    $scope.tables = response; //Assign data received to $scope.data
                });
            });
        }

        $scope.addNewRow = function () {
            new_row = $("tbody tr").first().html();
            $("tbody").append("<tr>"+new_row+"</tr>");
            $("tbody tr").last().find("input, select").val("");
        }

        $scope.get_tables = function (ID_database) {
            web_services.get('getTables', { ID: ID_database }).then(function (response) { //Async call to DBConnections factory
                return response; //Assign data received to $scope.data
            });
        }

        $('#tablesModal').on('show.bs.modal', function (event) {

            var button = $(event.relatedTarget) // Button that triggered the modal
            var database_ID = button.data('database_id') // Extract info from data-* attributes
            var database_name = button.data('database_name') 
            // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)

            var $scope = angular.element(modal).scope();

            $scope.$apply(function () {
                $scope.database_ID_active = database_ID;
            });

            modal.find('.modal-title').text('Tables of ' + database_name)
        })

        $(".query_DB").trigger('change');
        $(".query_DB").change(function () {
            $(".query_table").val("").trigger('change');
            if ($(this).val() != "") {
                $(".query_table option:not(:first)").hide();
                $(".query_table ." + $(this).val()).show();
            }
		});

        $(".query_table").change(function () {
            $(".query_columns").val("").trigger('change');
            if ($(this).val() != "") {
                $(".query_columns option:not(:first)").hide();
                $(".query_columns ." + $(this).val()).show();
            }
        });

        $('#where_check').change(function () {
            if ($('#where_check').is(':checked')) {
                enableElements($('#where_div').children());
            } else {
                disableElements($('#where_div').children());
            }
        });

        function disableElements(el) {
            for (var i = 0; i < el.length; i++) {
                el[i].disabled = true;
                disableElements(el[i].children);
            }
        }

        function enableElements(el) {
            for (var i = 0; i < el.length; i++) {
                el[i].disabled = false;

                enableElements(el[i].children);
            }
        }
    }
])