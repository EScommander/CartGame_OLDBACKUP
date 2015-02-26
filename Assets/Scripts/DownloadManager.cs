/* DownloadManager.cs
 * Coded by: Jeramy Zapotosky
 * GAM 52 
 */
using System;
using UnityEngine;
using System.Collections; 

class DownloadManager : MonoBehaviour 
{
//	//public GameObject avatar = null;
//
//	public static DownloadManager instance_ = null;
//
//	void Start()
//	{
//		instance_ = this;
//	}
//
//	public static DownloadManager GetInstance()
//	{
//		if(instance_ == null)
//		{
//			GameObject go = new GameObject("_DownloadManager");
//			instance_ = go.AddComponent<DownloadManager>();
//		}
//
//		return instance_;
//	}
//
//	public IEnumerator DownloadFromURL(string BundleURL, GameObject avatarRef) 
//	{
//		using (WWW www = new WWW(BundleURL)) 
//		{
//			yield return www;
//			if (www.error != null)
//			{
//				throw new Exception("WWW download had an error:" + www.error);
//			}
//			AssetBundle bundle = www.assetBundle;
//
//			GameObject go = (GameObject)Instantiate(bundle.mainAsset);
//			EquipableItem item = go.GetComponent<EquipableItem>();
//
//			if(item.bindSlot == BindSlot.HEAD)
//			{
//				Component[] transforms = avatarRef.GetComponentsInChildren<Transform>();
//				foreach(Transform t in transforms)
//				{
//					if(t.gameObject.name == "Head")
//					{
//						go.transform.position = t.position;
//						go.transform.parent = t;
//						go.transform.rotation = avatarRef.transform.rotation;
//						CharacterUI.equippedItems.Add(go);
//						break;
//					}
//				}
//			}
//			else if(item.bindSlot == BindSlot.SHOULDER)
//			{
//				Component[] transforms = avatarRef.GetComponentsInChildren<Transform>();
//				foreach(Transform t in transforms)
//				{
//
//					if(t.gameObject.name == "Right Upper Arm")
//					{
//						go.transform.position = t.position;
//						go.transform.parent = t;
//						go.transform.rotation = avatarRef.transform.rotation;
//						CharacterUI.equippedItems.Add(go);
//						break;
//					}
//
//					if(t.gameObject.name == "Left Upper Arm")
//					{
//						GameObject go2 = (GameObject)Instantiate(go);
//						go2.transform.position = t.position;
//						go2.transform.localScale = new Vector3(-1*go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
//						go2.transform.parent = t;
//						go2.transform.rotation = avatarRef.transform.rotation;
//						CharacterUI.equippedItems.Add(go2);
//					}
//				}
//
//			}
//			else if(item.bindSlot == BindSlot.KNEES)
//			{
//				Component[] transforms = avatarRef.GetComponentsInChildren<Transform>();
//				foreach(Transform t in transforms)
//				{
//
//					if(t.gameObject.name == "Right Lower Leg")
//					{
//						go.transform.position = t.position;
//						go.transform.parent = t;
//						go.transform.rotation = avatarRef.transform.rotation;
//						CharacterUI.equippedItems.Add(go);
//						break;
//					}
//
//					if(t.gameObject.name == "Left Lower Leg")
//					{
//						GameObject go2 = (GameObject)Instantiate(go);
//						go2.transform.position = t.position;
//						go2.transform.localScale = new Vector3(-1*go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z);
//						go2.transform.parent = t;
//						go2.transform.rotation = avatarRef.transform.rotation;
//						CharacterUI.equippedItems.Add(go2);
//					}
//				}
//
//			}
//			bundle.Unload(false);
//			
//		}
//	}
}
