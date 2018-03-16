Dolittle.namespace("Dolittle", {
    server: Dolittle.Singleton(function () {
        var self = this;

        this.target = "";

        function deserialize(data) {
            if (Dolittle.isArray(data)) {
                var items = [];
                data.forEach(function (item) {
                    items.push(deserialize(item));
                });
                return items;
            } else {
                for (var property in data) {
                    if (Dolittle.isArray(data[property])) {
                        data[property] = deserialize(data[property]);
                    } else {
                        var value = data[property];

                        if (Dolittle.isNumber(value)) {
                            data[property] = parseFloat(value);
                        } else {
                            data[property] = data[property];
                        }
                    }
                }
                return data;
            }
        }


        this.post = function (url, parameters) {
            var promise = Dolittle.execution.Promise.create();

            if (!Dolittle.Uri.isAbsolute(url)) {
                url = self.target + url;
            }

            var actualParameters = {};

            for (var property in parameters) {
                actualParameters[property] = JSON.stringify(parameters[property]);
            }

            $.ajax({
                url: url,
                type: "POST",
                dataType: 'json',
                data: JSON.stringify(actualParameters),
                contentType: 'application/json; charset=utf-8',
                complete: function (result) {
                    var data = $.parseJSON(result.responseText);
                    deserialize(data);
                    promise.signal(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    promise.fail(jqXHR);
                }
            });

            return promise;
        };

        this.get = function (url, parameters) {
            var promise = Dolittle.execution.Promise.create();

            if (!Dolittle.Uri.isAbsolute(url)) {
                url = self.target + url;
            }

            if (Dolittle.isObject(parameters)) {
                for (var parameterName in parameters) {
                    if (Dolittle.isArray(parameters[parameterName])) {
                        parameters[parameterName] = JSON.stringify(parameters[parameterName]);
                    }
                }
            }

            $.ajax({
                url: url,
                type: "GET",
                dataType: 'json',
                data: parameters,
                contentType: 'application/json; charset=utf-8',
                complete: function (result, textStatus) {
                    var data = $.parseJSON(result.responseText);
                    deserialize(data);
                    promise.signal(data);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    promise.fail(jqXHR);
                }
            });

            return promise;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.server = Dolittle.server;