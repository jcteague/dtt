define 'user_panel_view', ['general_view', 'config','navbar_view','breadcrumb_view','user_invitations_view'], (GeneralView, config, NavbarView,BreadcrumbView,UserInvitationsView) ->

    class UserPanelView extends GeneralView

        id: 'user-panel-container'
        initialize: ->
            @navbar = new NavbarView(model:@model)
            @breadcrumb = new BreadcrumbView(model:@model)
            @invitations = new UserInvitationsView(model:@model)
            console.log @model
        render: ->
            get_field = (field_name, data)->
                for field in data
                    if(field.name == field_name)
                        return field.value

            @$el.empty()
            @navbar.render().append_to @$el
            @breadcrumb.render().append_to @$el
            rooms_row = $("<div class='row'></div>")
            rooms_row.append '<h1 class="span12" >Your rooms</h1>'
            if @model.has('rooms')
                rooms = @model.get('rooms')
                for room in rooms
                    rooms_row.append """<div class='span4 hero-unit'><h2>#{get_field('name', room.data)}</h2><a href='#/room/#{get_field('id', room.data)}'>Manage</a><br/><a href='#/room/#{get_field('id', room.data)}/messages'>Go to room</a></div>"""
            else
                rooms_row.append """No rooms available at this moment"""
            @$el.append rooms_row
            invitations_row = $("""<div class='row' class="span12"></div>""")
            invitations_row.append """<h1 class="span12">Pending Invitations</h1>"""
            @invitations.render().append_to invitations_row
            @$el.append invitations_row
            @
