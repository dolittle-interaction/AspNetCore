describe("when checking element without format if element has format", function () {
    var formatter = null;
    var formatterBefore = null;
    var result;

    beforeEach(function () {
        var element = {
            nodeType: 1,
            attributes: {
                getNamedItem: sinon.stub().returns(null)
            }
        }

        formatterBefore = Dolittle.values.Formatter;
        Dolittle.values.Formatter = {
            getExtenders: sinon.stub().returns([])
        };

        formatter = Dolittle.values.stringFormatter.createWithoutScope();
        result = formatter.hasFormat(element);


    });

    afterEach(function () {
        Dolittle.values.Formatter = formatterBefore;
    });

    
    it("should not considered to have format", function () {
        expect(result).toBe(false);
    });
});