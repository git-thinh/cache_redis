////if (___para == null) ___para = {};
////___para['headers'] = {
////    url: 'https://vnexpress.net/chu-tich-vcci-noi-long-phong-toa-la-goi-kich-thich-kinh-te-lon-nhat-4087759.html',
////    method: 'GET'
////};
////___para['data'] = '';

var rs = {
    ok: false,
    error: '',
    data: {
        html: '',
        text: '',
        images: [],
        links: []
    },
    request: ___para
};


var o_html = ___api_call('request_async', ___para);
if (o_html.ok) {
    var o_html_2 = ___api_call('html_clean_01', {
        html: o_html.data,
        remove_first: ___para['remove_first'],
        remove_end: ___para['remove_end']
    });
    if (o_html_2.ok) {
        rs.ok = true;
        var htm = o_html_2.data;

        var o_link = ___api_call('html_export_links', { html: htm });
        if (o_link.ok) rs.data.links = o_link.data;

        var o_img = ___api_call('html_export_images', { html: htm });
        if (o_img.ok) {
            var imgs = o_img.data;
            if (imgs && imgs.length > 0) {
                for (var i = 0; i < imgs.length; i++) {
                    var id = new Date().getTime().toString();
                    var tag_img = imgs[i].html,
                        attr_img = imgs[i].attrs;
                    if (attr_img && attr_img['data-src']) {
                        //htm = htm.split(tag_img).join('\r\n [' + id + '] \r\n ');
                        htm = htm.split(tag_img).join('<br> [IMG] ' + attr_img['data-src'] + ' <br>');
                    }
                    //___log('img', tag_img);
                    //___log('img', htm);
                }
            }
            rs.data.images = imgs;
        }

        rs.data.html = htm;

        var o_text = ___api_call('html_to_text_01', {
            html: htm,
            remove_first: ___para['text_remove_first'],
            remove_end: ___para['text_remove_end']
        });
        if (o_text.ok) {
            rs.data.text = o_text.data;
            ___log('text', o_text.data);
        } else rs.error = o_text.error;

    } else rs.error = o_html_2.error;
} else rs.error = o_html.error;

return JSON.stringify(rs);