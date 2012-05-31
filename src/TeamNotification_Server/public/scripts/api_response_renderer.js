function syntaxHighlight(json) {
    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
        var cls = 'number';
        if (/^"/.test(match)) {
            if (/:$/.test(match)) {
                cls = 'key';
            } else {
                cls = 'string';
            }
        } else if (/true|false/.test(match)) {
            cls = 'boolean';
        } else if (/null/.test(match)) {
            cls = 'null';
        }
        return '<span class="' + cls + '">' + match + '</span>';
    });
}

var left_margin = 1;

var ApiResponseRenderer  = (function(module){
    module.begin_render=  function(){
        var json_text = $('#response_text').val();
        response = JSON.parse(json_text);
        $('#response').html(syntaxHighlight(JSON.stringify(response,null,'\t')));

        render(response);
    };

    var append_element_to_view = function(content){
        $('#response_view').append($('<p>').append(content));
    };

    var build_link = function(path) {
        return "http://localhost:3000/api_client/?path=" + path;
    };

    var render = function(response){
        var keys = Object.keys(response);

        for(var key_index =0; key_index < keys.length; key_index ++) {
            var key = keys[key_index];

            if(key === 'links') {
                render_links_for(response['links']);
            }else if(key === 'forms'){

            };
        }
    };

    var render_links_for = function(links){
        var link_names = Object.keys(links);

        for(var link_name_index =0; link_name_index < link_names.length; link_name_index ++){
            var link_name = link_names[link_name_index];
            var link = links[link_name];

            var link_url = build_link(link['href']);
            var link_html = $('<a>').attr('href',link_url).append(link_name);
            append_element_to_view(link_html);
        };
    };

    return module;
})({})


$(ApiResponseRenderer.begin_render)
