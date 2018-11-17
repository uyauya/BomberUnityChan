using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour 
	{
		public List<GameObject> Players = new List<GameObject> ();

		private int deadPlayers = 0;		//プレイヤーの死亡人数
		private int deadPlayerNumber = -1;	//死んだプレイヤーナンバー判別用

		//プレイヤー死亡判定
		public void PlayerDied (int playerNumber)
		{
			//プレイヤー死亡数を随時更新
			deadPlayers++;

			//プレイヤーが一人でも死んだら
			if (deadPlayers == 1)
			{
				//死んだプレイヤーのナンバーを更新して0.3秒後にCheckPlayersDeath処理
				deadPlayerNumber = playerNumber;
				Invoke ("CheckPlayersDeath", .3f);
			}
		}

		//プレイヤー死亡処理
		void CheckPlayersDeath ()
		{
			//死んだのプレイヤー1なら
			if (deadPlayers == 1)
			{ //Single dead player, he's the winner

				if (deadPlayerNumber == 1)
				{ //P1 dead, P2 is the winner
					Debug.Log ("Player 2 is the winner!");
				} else if (deadPlayerNumber == 2)
				{ //P2 dead, P1 is the winner
					Debug.Log ("Player 1 is the winner!");
				}
			} else
			{  //Multiple dead players, it's a draw
				Debug.Log ("The game ended in a draw!");
			}
		}
	}
