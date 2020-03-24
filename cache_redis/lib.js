﻿var ___fn = {};

function ___guid() {
    return 'xx-xxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function ___convert_unicode_to_ascii(str) {
    if (str == null || str.length == 0) return '';
    try {
        str = str.trim();
        if (str.length == 0) return '';

        var AccentsMap = [
            "aàảãáạăằẳẵắặâầẩẫấậ",
            "AÀẢÃÁẠĂẰẲẴẮẶÂẦẨẪẤẬ",
            "dđ", "DĐ",
            "eèẻẽéẹêềểễếệ",
            "EÈẺẼÉẸÊỀỂỄẾỆ",
            "iìỉĩíị",
            "IÌỈĨÍỊ",
            "oòỏõóọôồổỗốộơờởỡớợ",
            "OÒỎÕÓỌÔỒỔỖỐỘƠỜỞỠỚỢ",
            "uùủũúụưừửữứự",
            "UÙỦŨÚỤƯỪỬỮỨỰ",
            "yỳỷỹýỵ",
            "YỲỶỸÝỴ"
        ];
        for (var i = 0; i < AccentsMap.length; i++) {
            var re = new RegExp('[' + AccentsMap[i].substr(1) + ']', 'g');
            var char = AccentsMap[i][0];
            str = str.replace(re, char);
        }

        str = str
            .replace(/[\u0300-\u036f]/g, "")
            .replace(/đ/g, "d")
            .replace(/Đ/g, "D");

        str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
        str = str.replace(/ + /g, " ");

        str = str.toLowerCase();

    } catch (err_throw) {
        ;
    }

    return str;
}

function ___index(o) {
    if (o == null) o = {};
    var ids = [], utf8 = [];

    for (var col in o) {
        var isNumber = typeof o[col] == 'number';
        var val = o[col];
        if (col == 'id' && val != null) o[col] = Number(val);

        if (val != null && val != -1) {
            if (isNumber) ids.push(val);
            else utf8.push(val);
        }
    }

    o['#ids'] = ids.join(' ');
    o['#utf8'] = utf8.join(' ');
    o['#ascii'] = ___convert_unicode_to_ascii(o['#utf8']);
    o['#org'] = o['#ids'] + ' ' + o['#utf8'];

    return JSON.stringify(o);
}
