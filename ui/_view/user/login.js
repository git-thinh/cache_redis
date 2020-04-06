{
    data: function () {
        return {
            visible: false
        };
    },
    mounted: function () {
        var _self = this;
        console.log('vue.logint: mounted, role = ', _self.role___);
    },
    methods: {
        login: function() {
            ___login({
                ok: true,
                id: 617,
                str_full_name: "Nguyễn Thị Ngọc Chi",
                str_group_name: "Hỗ trợ khách hàng",
                str_pass: "vJtXGK/f/oX7E1VTR5af9Q==",
                str_possition: null,
                str_shop_name: "Hỗ trợ khách hàng",
                str_token: "61712345256c8047e018b2d64e7a53bdb26aa1188a00c8046da186748a276c05a1370e4b19c37826411c18ea7b9cb912458d18bf3a99545834bd079b1a8cca9c103305e",
                str_user_email: "nguyenthingocchi@f88.vn",
                str_user_name: "chintn",
                str_user_position: "0",
            });
        }
    }
}
