{
    data: function () {
        return {
            visible: true
        };
    },
    mounted: function () {
        var _self = this;
        console.log('vue_header_top === ', _self.objUser);
    },
    methods: {
        test_open_popup_view: function(event, view) {
            var _self = this;
            //console.log('vue_header_search event = ', event);
            view___dialog(event, view);
        }
    }
}