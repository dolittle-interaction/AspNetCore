describe("when asking if url is absolute and it is absolute http", function () {

    var result = Dolittle.Uri.isAbsolute("http://example.com");

    it("should be considered absolute", function () {
        expect(result).toBe(true);
    });


});