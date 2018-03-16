describe("when all properties are set and they already was", function () {
    var query = {
        foo: ko.observable(1),
        areAllParametersSet: ko.observable(true)
    };
    var paging = {
        size : 0,
        number : 0
    };

    var region = {};
    var pagingInfoType = null;
    var queryService = {
        execute: sinon.stub()
    };

    beforeEach(function () {
        pagingInfoType = Dolittle.read.PagingInfo;

        Dolittle.read.PagingInfo = {
            create: function () {
                return paging;
            }
        };

        Dolittle.read.Queryable.create({
            query: query,
            region: region,
            queryService: queryService,
            targetObservable: {}
        });

    });

    afterEach(function () {
        Dolittle.read.PagingInfo = pagingInfoType;
    });
    
    it("should not execute the query on the query service", function () {
        expect(queryService.execute.calledOnce).toBe(true);
    });
});