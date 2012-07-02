tobi = require 'tobi'
express = require 'express'
should = require 'should'

{server: server, handle_in_series: handle_in_series} = require('./helpers/specs_helper')

Authentication = require '../support/authentication'
auth = new Authentication()
app = express.createServer()
app.configure ->
	 app.use(auth.initializeAuth());	

app.get '/', auth.authenticate(), (req,res) ->
	res.send "done"


browser = tobi.createBrowser(app)

console.log "Passport test"
describe "Passport Authentication: ", ->

    beforeEach (done) ->
        handle_in_series server.start(), done

	describe "When an unauthenticated request is received", ->

		it 'should return a 401', (done)->
			browser.get '/', (res,$) ->
				res.should.have.status 401
				done()

	describe "When an Authentication Header is found", ->

		it "should return a 200 status code", ->

			browser.get '/',{headers:{"authorization": "Basic john:password"}}, (res,$) ->
				res.should.have.status 200
