var rs = {
    ok: false,
    error: '',
    data: {
        html: '',
        text: '',
        images: [],
        links: []
    },
    request: {}
};

if (___para == null) ___para = {};
___para['headers'] = {
    url: 'https://vnexpress.net/chu-tich-vcci-noi-long-phong-toa-la-goi-kich-thich-kinh-te-lon-nhat-4087759.html',
    method: 'GET'
};
___para['data'] = '';
rs.request = ___para;


var o_html = ___api_call('request_async', ___para);
if (o_html.ok) {
    var o_html_2 = ___api_call('html_clean_01', {
        html: o_html.data,
        //remove_first: '<h1 class="title-detail">',
        //remove_end: '<div class="list_link"',
        remove_first: ___para['remove_first'],
        remove_end: ___para['remove_end'],
    });
    if (o_html_2.ok) {
        rs.data.html = o_html_2.data;
        var o_text = ___api_call('html_to_text_01', { html: o_html_2.data });
        if (o_text.ok) {
            rs.data.text = o_text.data;
        } else rs.error = o_text.error;
    } else rs.error = o_html_2.error;
} else rs.error = o_html.error;

return JSON.stringify(rs);