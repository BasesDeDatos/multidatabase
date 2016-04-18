angular.module('manager', [])

.controller('managerCtrl', [
    '$scope',
    '$http',
    'web_services',
    function ($scope, $http, web_services) {

        $scope.DBConnections = false; //se inicia en false para que muestre el loader en la página
        $scope.databases = false;
        $scope.tables = false;
        $scope.columns = false;
        $scope.columnsxTable = function (id_table) {
            var return_listColumns = [];
            for (var key in $scope.columns) {
                // skip loop if the property is from prototype
                if (!$scope.columns.hasOwnProperty(key)) continue;

                var column = $scope.columns[key];
                if (column.ID_tabla == id_table){
                    return_listColumns.push({name : column.column_name});
                }
            }
            return return_listColumns;
        }

        web_services.get('getDBConnections').then(function (response) { //Async call to DBConnections factory
            $scope.DBConnections = response; //Assign data received to $scope.data
        });

        web_services.get('getDatabases').then(function (response) { //Async call to DBConnections factory
            $scope.databases = response; //Assign data received to $scope.data
        });

        web_services.get('getTables').then(function (response) { //Async call to DBConnections factory
            $scope.tables = response; //Assign data received to $scope.data
            $(".query_DB").trigger('change');
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
                port: $scope.port,
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
                web_services.get('getColumns').then(function (response) { //Async call to DBConnections factory
                    $scope.columns = response; //Assign data received to $scope.data
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
        $scope.dropTableModal = function (ID_table) {
            $scope.drop_table_ID = ID_table;
            $scope.dropTable();
        }

        $scope.addNewRow = function () {
            new_row = $("tbody tr").first().html();
            $("tbody").append("<tr>"+new_row+"</tr>");
            $("tbody tr").last().find("input, select").val("");
        }

        $scope.addNewRowTable = function () {
            new_row = $(".cont_query_table").html();
            $(".cont_new_query_table").append("<div class='margin-top-5'>" + new_row + "</div>");
            $(".cont_new_query_table .query_table").last().val("");
        }

        $scope.addNewRowColumn = function () {
            new_row = $(".cont_query_column").html();
            $(".cont_new_query_column").append("<div class='margin-top-5'>" + new_row + "</div>");
            $(".cont_new_query_column .query_column").last().val("");
            init_eventosChange_query;
        }

        $scope.addNewRowWhere = function () {
        }

        $scope.executeQuery = function () {
            var params = {
                query: $scope.query,
                source: $scope.source,
                tableXcolumn: [],
                filter: {
                    column: $scope.columnFilter,
                    method: $scope.methodFilter,
                    byValue: $scope.byValueFilter,
                },
                order: [],
            }

            $scope.columnsActive = [];

            $(".query_columns .query_column").each(function () {
                params.tableXcolumn.push({
                    table: $(this).find("option:selected").attr("id_table"),
                    column: $(this).find("option:selected").attr("id_column"),
                })
                $scope.columnsActive.push($(this).find("option:selected").attr("name_table")+"."+$(this).find("option:selected").attr("name_column"))
            });

            web_services.post("executeQuery", params, $scope).then(function (result) {
                if ($scope.RAW === true) {
                    $scope.queryResult = JSON.stringify(result, null, 4);
                } else {
                    $scope.queryResult = result;
                }
                console.log("FINALLY QUERY!!!")
            });
        }
        
        $scope.query_insertData = function () {
            var params = {
                source: $scope.source_insertData,
                valueXcolumn: [],
            }
            
            $(".table_body_columnsInsert input").each(function () {
                if($(this).is(':visible') ){
                    params.valueXcolumn.push({
                        value: $(this).val(),
                        column: $(this).attr("id_column"),
                    })
                }
            });

            web_services.post("insertRow", params, $scope).finally(function () {
                console.log("FINALLY QUERY!!!")
            });
        }

        $scope.queryAcutalizar = function () {
            var params = {
                source: $scope.source_actualizar,
                column: $scope.column_actualizar,
                value: $scope.value_actualizar,

                filter: { //where 
                    column: $scope.columnFilter_actualizar,
                    method: $scope.methodFilter_actualizar,
                    byValue: $scope.byValueFilter_actualizar,
                }
            }

            $scope.columnsActive = [];
            web_services.post("updateQuery", params, $scope).then(function (result) {
            console.log("FINALLY QUERY!!!")
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

        $('#queryModal').on('show.bs.modal', function (event) {

            var button = $(event.relatedTarget) // Button that triggered the modal
            var database_ID = button.data('database_id') // Extract info from data-* attributes
            var database_name = button.data('database_name')
            // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
            // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
            var modal = $(this)

            var $scope = angular.element(modal).scope();
            $scope.$apply(function () {
                $scope.queryResult = false;
                tables = "";
                tables_len = $(".query_tables .query_table").length;
                columns = "";
                columns_len = $(".query_columns .query_column").length;
                where = ";";
                $(".query_tables .query_table").each(function (index, element) {
                    tables +=
                        "    " + $(this).find("option:selected").attr("name_table") + (tables_len - 1 == index ? "" : "<br>INNER JOIN <br>")
                });

                $(".query_columns .query_column").each(function (index, element) {
                    columns +=
                        "    " + $(this).find("option:selected").attr("name_table")
                        + "." +
                        $(this).find("option:selected").attr("name_column") + (columns_len - 1 == index ? "<br>" : ", <br>")
                });

                if ($('#where_check').is(':checked')) {
                    var column = $("#executeQuery_column").find("option:selected").attr("column_name");
                    where = "<br><br>WHERE " + column + " " + $scope.methodFilter + " " + $scope.byValueFilter + ";";
                }

                modal.find('.modal-title')
                .html(
                    'Result of <br>' +
                    '<pre>' +
                        '<code>' +
                            "SELECT <br>" +
                            columns +
                            '<br>FROM <br>' +
                            tables +
                            where +
                        '</code>' +
                    '</pre>'
                );
            });
        })


        init_eventosChange_query();
        function init_eventosChange_query(){
            $(".query_DB").change(function () {
                $(".query_table").val("").trigger('change');
                if ($(this).val() != "") {
                    $(".query_table option:not(:first)").hide();
                    $(".query_table ." + $(this).val()).show();
                }
            });

            $(".query_table").change(function () {
                $(".query_column option:not(:first)").hide();
                var select_ID = "";
                $(".query_table").each(function () {
                    select_ID += $(this).val()
                    if ($(this).val() != "") {
                        $(".query_column ." + $(this).val()).show();
                    }
                })

                //Si no hay ninguna opcion seleccionada en ningun select 
                if (select_ID != "") {
                    $(".query_column").val("");
                }
            });
        }
        
        $('.query_where_columns').change(function () {
            switch ($(this).find("option:selected").attr("type")) {
                case "string": $('.byValueFilter').attr("type", "text").attr("step", ""); break;
                case "entero": $('.byValueFilter').attr("type", "number").attr("step", "1"); break;
                case "doble": $('.byValueFilter').attr("type", "number").attr("step", "any"); break;
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
                el[i].val() = "";
                disableElements(el[i].children);
            }
        }

        function enableElements(el) {
            for (var i = 0; i < el.length; i++) {
                el[i].disabled = false;

                enableElements(el[i].children);
            }
        }
       
        $('.actualizar_datacolumn').change(function () {
            var typeColumn = $(this).find("option:selected").attr("type");
            $("#input_actualizar").hide();
            if (typeColumn != "") {
                switch (typeColumn) {
                    case "string": $("#input_actualizar").show().attr("type", "text").attr("step", ""); break;
                    case "entero": $("#input_actualizar").show().attr("type", "number").attr("step", "1"); break;
                    case "doble": $("#input_actualizar").show().attr("type", "number").attr("step", "any"); break;
                }
            }
        });

        $('.query_table_insertdata').change(function () {
            var id_tabla = $(this).val();
            $(".query_columninsertdata").hide();
            $(".query_columninsertdata input").removeAttr("required");
            if (id_tabla != "") {
                $(".query_columninsertdata." + id_tabla).show();

                $(".query_columninsertdata." + id_tabla + ":visible input").each(function () {

                    var is_required = $(this).attr("is_required");
                    if (is_required == "true") {
                        $(this).attr("required", is_required);
                    } else {
                        $(this).removeAttr("required");
                    }
  
                });
            }
        });

        $('.actualizar_datacolumn').change(function () {
            var typeColumn = $(this).find("option:selected").attr("type");
            $("#input_actualizar").hide();
            if (typeColumn != "") {
                switch (typeColumn) {
                    case "string": $("#input_actualizar").show().attr("type", "text").attr("step", ""); break;
                    case "entero": $("#input_actualizar").show().attr("type", "number").attr("step", "1"); break;
                    case "doble": $("#input_actualizar").show().attr("type", "number").attr("step", "any"); break;
                }
            }
        });
        
    }
   
])