extends Area2D

@export var chao: TileMapLayer
@export var thorn: Node2D

var _sprite: AnimatedSprite2D
var _players_in_area: int = 0
var can_help: int = 0

func _ready():
	_sprite = $AnimatedSprite2D

	connect("body_entered", Callable(self, "_on_body_entered"))
	connect("body_exited", Callable(self, "_on_body_exited"))

	if chao:
		chao.enabled = false
	if thorn:
		thorn.visible = false


func _on_body_entered(body: Node) -> void:
	if body.is_in_group("PlayerGroup"):
		_players_in_area += 1
		_ativar_botao()
		print("Entrou: %s | Total na área: %d" % [body.name, _players_in_area])
		var player = get_node_or_null("../Player")


func _on_body_exited(body: Node) -> void:
	if body.is_in_group("PlayerGroup"):
		_players_in_area -= 1
		if _players_in_area <= 0:
			_players_in_area = 0
			_desativar_botao()
			print("Saiu: %s | Botão desativado (0 players na área)" % body.name)
		else:
			print("Saiu: %s | Ainda há %d players na área" % [body.name, _players_in_area])


func _ativar_botao():
	_sprite.play("ButtonClicked")
	if chao:
		chao.enabled = true
	if thorn:
		thorn.visible = true
	# print("Funciona")


func _desativar_botao():
	_sprite.play("ButtonDefault")
	if chao:
		chao.enabled = false
	if thorn:
		thorn.visible = false
	# print("Desativado")
