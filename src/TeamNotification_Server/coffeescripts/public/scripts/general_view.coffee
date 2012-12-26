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
            
        get_link: (rel, links_collection)->
            for link in links_collection
                if(link.rel == rel)
                    return """<a href="##{link.href}">#{link.name}</a>"""
                    
        get_field: (field, collection)->
            for obj in collection
                if(obj.name = field)
                    return obj.value
