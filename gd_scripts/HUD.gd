extends Node2D  # ou CanvasLayer, se a HUD for fixa na tela

@onready var meu_label: Label = $Label # Altere para o caminho correto do Label na árvore
var _player: CharacterBody2D = null

func _ready():
	meu_label.text = "0"

func set_player(player):
	_player = player
	if _player != null:
		_player.connect("pontuacao_atualizada", Callable(self, "_on_pontuacao_atualizada"))
		_player.connect("clones_atualizados", Callable(self, "_on_clones_atualizados"))
		update_clones_display()
		print("ClonesCurrent:", _player.clones_current, "ClonesMax:", _player.max_clones)
	else:
		push_error("set_player recebeu null!")

func _on_pontuacao_atualizada(nova_pontuacao):
	# Se quiser exibir a pontuação, descomente a linha abaixo:
	# meu_label.text = str(nova_pontuacao)
	pass

func _on_clones_atualizados(clones_disponiveis):
	update_clones_display()

func update_clones_display():
	if _player and meu_label:
		var clones_restantes = _player.max_clones - _player.clones_current
		meu_label.text = str(clones_restantes)
		print("MaxClones:", _player.max_clones)
		print("ClonesCurrent:", _player.clones_current)

func _exit_tree():
	if _player:
		_player.disconnect("pontuacao_atualizada", Callable(self, "_on_pontuacao_atualizada"))
		_player.disconnect("clones_atualizados", Callable(self, "_on_clones_atualizados"))
