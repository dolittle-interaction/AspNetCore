Dolittle.namespace("Dolittle.read", {
    PagingInfo: Dolittle.Type.extend(function (size, number) {
        this.size = size;
        this.number = number;
    })
});