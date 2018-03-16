Dolittle.namespace("Dolittle.read", {
    QueryTask: Dolittle.tasks.LoadTask.extend(function (query, paging, taskFactory) {
        var url = "/Dolittle/Query/Execute?_q=" + query._generatedFrom;
        var payload = {
            descriptor: {
                nameOfQuery: query._name,
                generatedFrom: query._generatedFrom,
                parameters: query.getParameterValues()
            },
            paging: {
                size: paging.size,
                number: paging.number
            }
        };

        this.query = query._name;
        this.paging = payload.paging;

        var innerTask = taskFactory.createHttpPost(url, payload);

        this.execute = function () {
            var promise = innerTask.execute();
            return promise;
        };
    })
});