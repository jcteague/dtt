define 'session_mng', ['config', 'jquery', 'cookie'], (Config, jquery, cookie) ->

    is_white_path = (path) ->
        parts = path.split('/#')
        if(parts.length > 1)
            relevant = parts[1]
            found = false
            console.log relevant
            for white_path in Config.whitelist
                if( relevant.indexOf(white_path) == 0 )
                    return true
            return false
        return true

    check_cookie = () ->
        if $.cookie('authtoken') == null || $.cookie('authtoken') == ''
            if(!is_white_path(window.location.href))
                window.location = '#/'
        else if(is_white_path(window.location.href))
            if(is_white_path(window.location.href))
                window.location = '#/user'
                
    $( ()->
        setInterval( check_cookie, 1500)
        check_cookie() ) 
