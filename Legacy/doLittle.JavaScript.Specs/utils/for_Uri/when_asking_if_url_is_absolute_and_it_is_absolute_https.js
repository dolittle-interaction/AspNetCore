describe("when asking if url is absolute and it is absolute https", function () {

    var result = Dolittle.Uri.isAbsolute("https://example.com");

    it("should be considered absolute", function () {
        expect(result).toBe(true);
    });


});