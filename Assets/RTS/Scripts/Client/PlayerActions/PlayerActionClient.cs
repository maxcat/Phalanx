using UnityEngine;
using System.Collections;

public partial class PlayerAction
{
    public virtual bool TouchSelection(RaycastHit[] hits, Touch touch, out GameObject selectedObject)
    {
        bool result = false;
        selectedObject = null;

        if ((TargetOFlagMask & ((int)TargetOFlag.HeroYourself | (int)TargetOFlag.HeroFriend | (int)TargetOFlag.HeroEnemy)) != (int)TargetOFlag.Empty)
        {
            result = HeroSelection(hits, touch, out selectedObject);
            if (selectedObject != null)
            {
                return result;
            }
        }

        if ((TargetOFlagMask & ((int)TargetOFlag.StructureFriend | (int)TargetOFlag.StructureEnemy)) != (int)TargetOFlag.Empty)
        {
            result = StructureSelection(hits, touch, out selectedObject);
            if (selectedObject != null)
            {
                return result;
            }
        }

        if ((TargetOFlagMask & ((int)TargetOFlag.CreepFriend | (int)TargetOFlag.CreepEnemy)) != (int)TargetOFlag.Empty)
        {
            result = CreepSelection(hits, touch, out selectedObject);
            if (selectedObject != null)
            {
                return result;
            }
        }

        if ((TargetOFlagMask & (int)TargetOFlag.Grid) != (int)TargetOFlag.Empty)
        {
            result = GridSelection(hits, touch, out selectedObject);
            if (selectedObject != null)
            {
                return result;
            }
        }
        
        return result;
    }


    protected bool HeroSelection(RaycastHit[] hits, Touch touch, out GameObject selectedObject)
    {
        selectedObject = null;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            HeroClientController hero = hit.transform.GetComponent<HeroClientController>();

            if (hero != null)
            {
                int objectMask = 0;

                if (hero.PlayerID == PlayerDataSingleton.Instance.PlayerID)
                {
                    objectMask |= (int)TargetOFlag.HeroYourself;
                }
                
                if ((TargetOFlagMask & objectMask) == (int)TargetOFlag.HeroYourself)
                {
                    selectedObject = hero.gameObject;
                }
                else
                {
                    if (hero.TeamID == PlayerDataSingleton.Instance.TeamID)
                    {
                        objectMask |= (int)TargetOFlag.HeroFriend;
                    }
                    else
                    {
                        objectMask |= (int)TargetOFlag.HeroEnemy;
                    }
                    
                    if ((TargetOFlagMask & objectMask) != (int)TargetOFlag.Empty)
                    {
                        selectedObject = hero.gameObject;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    if (selectedObject != null)
                    {
                        //TODO: if legal object send action
                        Debug.Log("HeroSelection END " + hero.ObjectID);
                    }

                    return true;
                }
                else
                {
                    if (selectedObject != null)
                    {
                        //TODO: if legal object update visuals
                    }

                    return false;
                }
            }
        }
        
        return false;
    }

    protected bool StructureSelection(RaycastHit[] hits, Touch touch, out GameObject selectedObject)
    {
        selectedObject = null;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            StructureClientController structure = hit.transform.GetComponent<StructureClientController>();

            if (structure != null)
            {
                int objectMask = 0;
                
                if (structure.TeamID == PlayerDataSingleton.Instance.TeamID)
                {
                    objectMask |= (int)TargetOFlag.StructureFriend;
                }
                else
                {
                    objectMask |= (int)TargetOFlag.StructureEnemy;
                }

                if ((TargetOFlagMask & objectMask) != (int)TargetOFlag.Empty)
                {
                    selectedObject = structure.gameObject;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    if (selectedObject != null)
                    {
                        //TODO: if legal object send action
                        Debug.Log("StructureSelection END " + structure.ObjectID);
                    }

                    return true;
                }
                else
                {
                    if (selectedObject != null)
                    {
                        //TODO: if legal object update visuals
                    }

                    return false;
                }
            }
        }

        return false;
    }

    protected bool CreepSelection(RaycastHit[] hits, Touch touch, out GameObject selectedObject)
    {
        selectedObject = null;

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            CreepClientController creep = hit.transform.GetComponent<CreepClientController>();

            if (creep != null)
            {
                int objectMask = 0;

                if (creep.TeamID == PlayerDataSingleton.Instance.TeamID)
                {
                    objectMask |= (int)TargetOFlag.CreepFriend;
                }
                else
                {
                    objectMask |= (int)TargetOFlag.CreepEnemy;
                }

                if ((TargetOFlagMask & objectMask) != (int)TargetOFlag.Empty)
                {
                    selectedObject = creep.gameObject;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    if (selectedObject != null)
                    {
                        //TODO: if legal object send action
                        Debug.Log("CreepSelection END " + creep.ObjectID);
                    }

                    return true;
                }
                else
                {
                    if (selectedObject != null)
                    {
                        //TODO: if legal object update visuals
                    }

                    return false;
                }
            }
        }

        return false;
    }

    protected bool GridSelection(RaycastHit[] hits, Touch touch, out GameObject selectedObject)
    {
        selectedObject = null;
        
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            MapSingleton mapReceiver = hit.transform.GetComponent<MapSingleton>();

            if (mapReceiver != null)
            {
                selectedObject = mapReceiver.gameObject;
                
                if (touch.phase == TouchPhase.Ended)
                {
                    if (selectedObject != null)
                    {
                        Debug.Log("GridSelection END");
                        //TODO: if legal object send action
                    }

                    return true;
                }
                else
                {
                    if (selectedObject != null)
                    {
                        //TODO: if legal object update visuals
                    }

                    return false;
                }
            }
        }

        return false;
    }
}
