require "System.Wrap"
luanet.load_assembly("UnityEngine")

object			= System.Object
Type			= System.Type
Object          = UnityEngine.Object
GameObject 		= UnityEngine.GameObject
Transform 		= UnityEngine.Transform
MonoBehaviour 	= UnityEngine.MonoBehaviour
Component		= UnityEngine.Component
Application		= UnityEngine.Application
SystemInfo		= UnityEngine.SystemInfo
Screen			= UnityEngine.Screen
Camera			= UnityEngine.Camera
Material 		= UnityEngine.Material
Renderer 		= UnityEngine.Renderer
AsyncOperation	= UnityEngine.AsyncOperation

CharacterController = UnityEngine.CharacterController
SkinnedMeshRenderer = UnityEngine.SkinnedMeshRenderer
Animation		= UnityEngine.Animation
AnimationClip	= UnityEngine.AnimationClip
AnimationEvent	= UnityEngine.AnimationEvent
AnimationState	= UnityEngine.AnimationState
Input			= UnityEngine.Input
KeyCode			= UnityEngine.KeyCode
AudioClip		= UnityEngine.AudioClip
AudioSource		= UnityEngine.AudioSource
Physics			= UnityEngine.Physics
Light			= UnityEngine.Light
LightType		= UnityEngine.LightType
ParticleEmitter	= UnityEngine.ParticleEmitter
Space			= UnityEngine.Space
CameraClearFlags= UnityEngine.CameraClearFlags
RenderSettings  = UnityEngine.RenderSettings
MeshRenderer	= UnityEngine.MeshRenderer
WrapMode		= UnityEngine.WrapMode
QueueMode		= UnityEngine.QueueMode
PlayMode		= UnityEngine.PlayMode
ParticleAnimator= UnityEngine.ParticleAnimator
--TouchPhase 		= UnityEngine.TouchPhase
AnimationBlendMode = UnityEngine.AnimationBlendMode

function print(...)	
	local arg = {...}	
	local t = {}	
	
	for i,k in ipairs(arg) do
		table.insert(t, tostring(k))
	end
	
	local str = table.concat(t)	
	--Debugger.Log(str)
end


--require "strict"
--require "memory"
require "System.class"
require "System.Math"
--require "System.Layer"
require "System.List"
require "System.Time"
require "System.Event"
--require	"System.sqlite"
require "System.Timer"

require "System.Vector3"
require "System.Vector2"
require "System.Quaternion"
--require "System.Vector4"
--require "System.Raycast"
require "System.Color"
--require "System.Touch"
--require "System.Ray"
--require "System.Plane"
--require "System.Bounds"

--require "System.Coroutine"


function traceback(msg)
	msg = debug.traceback(msg, 2)
	return msg
end

function LuaGC()
  local c = collectgarbage("count")
  --Debugger.Log("Begin gc count = {0} kb", c)
  collectgarbage("collect")
  c = collectgarbage("count")
  --Debugger.Log("End gc count = {0} kb", c)
end

--[[function ShowUI(name, OnLoad)
	UIBase.LoadUI(name, OnLoad)
end--]]

--unity 对象判断为空, 如果你有些对象是在c#删掉了，lua 不知道
--判断这种对象为空时可以用下面这个函数。
function IsNil(uobj)
	return uobj == nil or uobj:Equals(nil)
end

-- isnan
function isnan(number)
	return not (number == number)
end

function string:split(sep)
	local sep, fields = sep or ",", {}
	local pattern = string.format("([^%s]+)", sep)
	self:gsub(pattern, function(c) table.insert(fields, c) end)
	return fields
end

function GetDir(path)
	return string.match(fullpath, ".*/")
end

function GetFileName(path)
	return string.match(fullpath, ".*/(.*)")
end

