
var obj = {
    "str_user_name": "",
    "str_pass_word": ""
};
var valid = {
    "str_user_name": {
        "name": "NOT_EMPTY",
        "message": "Vui lòng nhập thông tin Tên tài khoản"
    },
    "str_pass_word": {
        "para": null,
        "name": "NOT_EMPTY",
        "message": "Vui lòng nhập thông tin mật khẩu"
    }
};

/*[START]*/

var ERRORS = [];
for (var key in obj) {
    if (valid[key] != null) {
        var val = obj[key];
        var cf = valid[key];
        var para = cf.para;
        switch (cf.name) {
            case 'NOT_EMPTY':
                if (val == null || val.toString().trim().length == 0)
                    ERRORS.push(cf);
                break;
            case 'GREATER_THAN':
                break;
        }
    }
}

/*[COMPLETE]*/

return JSON.stringify(ERRORS);