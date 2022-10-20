using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDemoScene : MonoBehaviour 
{
	public UnityEngine.UI.Text mExplosionName;

	public List<ParticleSystem> mExplosionList = new List<ParticleSystem> ();

	public LayerMask mShootLayerMask;

	public int mCurrentIndex = 0;

    void Start()
    {
        mCurrentIndex = 0;
        mExplosionName.text = mExplosionList[mCurrentIndex].name;
    }

    public void OnClickPre()
	{
		mCurrentIndex--;

		if ( mCurrentIndex < 0 )
			mCurrentIndex = mExplosionList.Count -  1;

		mExplosionName.text = mExplosionList [mCurrentIndex].name;
	}


	public void OnClickNext()
	{
		mCurrentIndex++;

		if ( mCurrentIndex >= mExplosionList.Count )
			mCurrentIndex = 0;

		mExplosionName.text = mExplosionList [mCurrentIndex].name;
	}

	void Update()
	{
		if( Input.GetMouseButtonDown( 0 ) == true )
		{
			Ray checkRay = Camera.main.ScreenPointToRay ( Input.mousePosition );

			RaycastHit hitInfo;
			if ( Physics.Raycast (checkRay, out hitInfo, 100f, mShootLayerMask ) )
			{
				ParticleSystem obj = Instantiate (mExplosionList [mCurrentIndex]) as ParticleSystem;

				obj.transform.position = hitInfo.point;
                obj.gameObject.SetActive(true);
				//obj.Play ();
			}
		}

        if (Input.GetKeyDown(KeyCode.LeftArrow) == true)
            OnClickPre();

        if (Input.GetKeyDown(KeyCode.RightArrow) == true)
            OnClickNext();


	}


}
