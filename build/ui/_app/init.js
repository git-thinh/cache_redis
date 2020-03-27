var ___APP, ___VIEW = {}, ___COM = {}, ___HTML = {};
var ___DATA = {
    view___sidebar_left: null,
    view___sidebar_right: null,
    view___header_nav: null,
    view___header_breadcrumb: null,
    view___header_tab: null,
    view___header_filter: null,
    view___main_left: null,
    view___main_body: null,
    view___footer: null
};

var ___MIXIN = {
    props: [],
    computed: {
        view_id: function () { return this.$vnode.componentOptions.tag; }
    },
    created: function () {
        var _self = this;
        //console.log(_self.view_id);
    },
    mounted: function () {
        var _self = this;
        var el = _self.$el;
        classie.add(el, _self.view_id);
        //console.log(el);
    },
    methods: {
    },
    watch: {
        $data: {
            handler: function (val, oldVal) {
                var _self = this;
                //console.log('WATCH: ' + _self.view_id);
                Vue.nextTick(function () {
                    var el = _self.$el;
                    classie.add(el, _self.view_id);
                });
            },
            deep: true
        }
    },
};

var view___init = (callback) => {
    fetch('_view/config.json').then(r => r.json()).then(async cf_ => {
        ___VIEW = cf_;
        var fets = [];
        for (var vn in cf_) {
            if (cf_[vn] && cf_[vn].views) {
                cf_[vn].views.forEach((vi) => {
                    fets.push(fetch(('_view/' + vn + '/' + vi.name + '.html').toLowerCase()));
                    fets.push(fetch(('_view/' + vn + '/' + vi.name + '.js').toLowerCase()));
                    fets.push(fetch(('_view/' + vn + '/' + vi.name + '.css').toLowerCase()));
                });
            }
        }

        var results = await Promise.all(fets).then(async fs => {
            var a = [];
            for (var i = 0; i < fs.length; i++) {
                var r = fs[i];
                var type = r.url.substr(r.url.length - 2, 2);
                var p = r.url.split('/');
                var scope = p[p.length - 2];
                var api = p[p.length - 1].split('.')[0];
                var key = scope + '___' + api;
                var index = -1;

                //console.log(r.url, r.ok);

                if (type == 'js') {
                    if (r.ok) text = await r.text();
                    else text = '{ mixins: [___MIXIN], template: "<div></div>", data: function () { return {}; }, mounted: function () {}, methods: {} }';

                    text = text.trim().substr(1);
                    text = '___COM["' + key + '"] = { mixins: [___MIXIN], template: ___HTML["' + key + '"], \r\n ' + text + ' \r\n ' +
                        'Vue.component("' + key + '", ___COM["' + key + '"]); \r\n ';
                    var url_js = URL.createObjectURL(new Blob([text], { type: 'text/javascript' }));

                    index = ___VIEW[scope].views.findIndex(function (o) { return o.name == api; });
                    //console.log(scope, api, index);
                    if (index != -1) ___VIEW[scope].views[index].url_js = url_js;

                    a.push({ key: key, scope: scope, api: api, type: 'js', url_js: url_js });
                } else if (r.ok) {
                    text = await r.text();
                    switch (type) {
                        case 'ml': // html
                            ___HTML[key] = text;
                            break;
                        case 'ss': // css
                            //var url_css = '/_view/' + scope + '/' + api + '.css';
                            var url_css = URL.createObjectURL(new Blob([text], { type: 'text/css' }));
                            index = ___VIEW[scope].views.findIndex(function (o) { return o.name == api; });
                            console.log(scope, api, index);
                            if (index != -1) ___VIEW[scope].views[index].url_css = url_css;

                            var el = document.createElement('link');
                            el.setAttribute('rel', 'stylesheet');
                            el.setAttribute('href', url_css);
                            el.setAttribute('id', key + '___css');
                            document.getElementsByTagName('head')[0].appendChild(el);
                            break;
                    }
                }
            }
            //console.log('a === ', a);
            return a;
        }).then(async a => {
            var js_add = await Promise.all(a.map(it => {
                return new Promise((resolve, reject) => {
                    var el = document.createElement('script');
                    el.setAttribute('src', it.url_js);
                    el.setAttribute('id', it.key + '___js');
                    el.onload = function () {
                        resolve(it);
                    };
                    document.getElementsByTagName('head')[0].appendChild(el);
                });
            }));
            return js_add;
        });

        //console.log(JSON.stringify(results));
        results.forEach(r => {
            var index = ___VIEW[r.scope].views.findIndex(function (o) { return o.name == r.api; });
            if (index != -1) {
                var it = ___VIEW[r.scope].views[index];
                it.key = r.key;
                if (it.area_init != null && it.area_init.length > 0) {
                    ___DATA['view___' + it.area_init] = r.key;
                    console.log('VIEW___INIT: ' + it.area_init + ' ----> ' + r.key);
                }
            }
        });

        callback({ ok: true });
    }).catch((err) => callback({ ok: false, message: err.message }));
};

window.addEventListener("hashchange", function (h) {
    var old_hash = new URL(h.oldURL).hash;
    var new_hash = location.hash;
    if (old_hash && old_hash.indexOf('#!') == 0) old_hash = old_hash.substr(2);
    if (new_hash && new_hash.indexOf('#!') == 0) new_hash = new_hash.substr(2);
    console.log('HASH_CHANGE: ' + old_hash + ' -> ', new_hash);
    view___load(new_hash, old_hash);
}, false);

const view___load = (view, view_called) => {
    if (view == null || view.length == 0 || ___HTML[view] == null) return;

    console.log('VIEW___LOAD: ' + view);

    ___APP.view___main_body = view;
};

view___init((m) => {
    console.log('VIEW_INIT = ', m);
    if (m.ok == false) return;

    //___DATA.view___main_body = 'table___table_basic';

    ___APP = new Vue({
        el: '#app',
        data: function () { return ___DATA; }
    });
});