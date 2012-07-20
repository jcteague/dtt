define 'general_view', ['backbone'], (Backbone) ->

    class GeneralView extends Backbone.View
        id: 'view_id'
        events: () ->
            return
        initialize: ->
            return
        render: ->
            return
        listen_to:(param) ->
            return
        append_to: (parent) ->
            @$el.appendTo parent

        createCookie: (name,value,days) ->
            if (days) 
                date = new Date()
                date.setTime(date.getTime()+(days*24*60*60*1000))
                expires = "; expires="+date.toGMTString()
            else
                expires = ""
                document.cookie = name+"="+value+expires+"; path=/"

        readCookie: (name) ->
            nameEQ = name + "="
            ca = document.cookie.split(';')
            
            i=0
            while i < ca.length
                c = ca[i]
                while (c.charAt(0)==' ')
                    c = c.substring(1,c.length)
                if (c.indexOf(nameEQ) == 0) 
                    return unescape c.substring(nameEQ.length,c.length)
                i++
            return null
