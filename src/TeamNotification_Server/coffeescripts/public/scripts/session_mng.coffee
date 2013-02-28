define 'session_mng', ['config', 'jquery', 'cookie'], (Config, jquery, cookie) ->

    is_white_path = (path) ->
        parts = path.split('/#')
        if(parts.length > 1)
            relevant = parts[1]
            if( relevant == '/' || relevant =='')
                return true
            for white_path in Config.whitelist
                console.log white_path
                if( relevant.indexOf(white_path) == 0 )
                    return true
            return false
        return true

    check_cookie = () ->
        if $.cookie('authtoken') == null || $.cookie('authtoken') == ''
            if(!is_white_path(window.location.href))
                #console.log window.location.href
                window.location = '#/'
        else 
            if(is_white_path(window.location.href))
                #console.log 'to User'
                window.location = '#/user'
                
    $( ()->
        window.setInterval( check_cookie, 100)
        check_cookie() ) 
