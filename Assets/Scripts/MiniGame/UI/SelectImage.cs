using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectImage : MonoBehaviour
{
    // 상태
    public MiniGamePlayerState _State;
    
    // 설명
    [SerializeField] private Text _Explain;

    // 선택 버튼
    [SerializeField] private SelectBtn _Button;

    // 활성화 되었을 때
    private void OnEnable()
    {
        InitializeData();
    }

    // 초기화
    private void InitializeData()
    {
        // 설명
        ExplainUpdate();

        // 버튼 
        ChooseBtnUpdate();
    }
    
    // 선택창 설명 설정
    private void ExplainUpdate()
    {
        switch (_State)
        {
            // 1날개
            case MiniGamePlayerState.Angel: _Explain.text = "날개돼지\n 천계에서 온 돼지, 손에 든것은 사탕이다."; break;

            // 2아롱
            case MiniGamePlayerState.Arong: _Explain.text = "아롱이\n 내가 좋아하는 강아지다"; break;

            // 3 새끼1
            case MiniGamePlayerState.Baby1: _Explain.text = "복실이\n 아롱이의 새끼"; break;

            // 4새끼2
            case MiniGamePlayerState.Baby2: _Explain.text = "삼쨩\n 점이세개가 박힌 강아지"; break;

            // 5새끼3
            case MiniGamePlayerState.Baby3: _Explain.text = "마롱이\n 아롱이를 아주많이 닮은 아롱이의 새끼"; break;

            // 6새끼4
            case MiniGamePlayerState.Baby4: _Explain.text = "이치쨩\n 아롱이의 새끼들중 가장 덩치가 크다"; break;

            // 7악어새
            case MiniGamePlayerState.Bird: _Explain.text = "악어새\n '모두들 내가 악어 입속을 왔다갔다해서 악어새인줄 알지.. 후후'"; break;

            // 8흑돼지
            case MiniGamePlayerState.Black: _Explain.text = "흑돼지\n 제주도에서 자주 볼 수 있는 까만 돼지"; break;

            // 9튀는애
            case MiniGamePlayerState.Bouncing: _Explain.text = "쾅쾅쿠광쾅쾅쾅\n 몸에 페인트가 묻어 지우기위해 이리저리 구르는 돼지"; break;

            // 10황금동전
            case MiniGamePlayerState.GoldendCoin: _Explain.text = "황금돼지의 황금동전\n 그냥 동전"; break;

            // 11겁쟁이
            case MiniGamePlayerState.Coward: _Explain.text = "겁쟁이 돼지\n 겁이 많지만 힘은 가장 쌘 슈퍼돼지 후보1번"; break;

            // 12악어
            case MiniGamePlayerState.Croco: _Explain.text = "악어\n 심연속에 잠든 악마.. 악어..!"; break;

            // 13악어주인
            case MiniGamePlayerState.CrocoMaster: _Explain.text = "악어주인\n 악어를 마음대로 부리는 마법돼지!"; break;

            // 14까마귀
            case MiniGamePlayerState.Crow: _Explain.text = "괴물 까마귀\n 덩치는 커도 아직 1살인 아기이다."; break;

            // 15파괴자
            case MiniGamePlayerState.Destroyer: _Explain.text = "파괴자 돼지\n '때린다.. 부순다.. 파괴한다..!!"; break;

            // 16돼지인형옷
            case MiniGamePlayerState.Doll: _Explain.text = "돼지인형 옷\n 아무도 몰랐겠지만 사실 이녀석은 사람이다."; break;

            // 17오리
            case MiniGamePlayerState.Duck: _Explain.text = "미스 덕스엑\n 더럽게 떽떽거리는 고등학생, 그림은 잘그린다."; break;

            // 18에스더
            case MiniGamePlayerState.Esther: _Explain.text = "대단한 돼지 에스더\n 인간과 돼지들의 사이를 회복시켜줄 혁명의 한줄기의 빛"; break;

            // 19복돼지
            case MiniGamePlayerState.Golden: _Explain.text = "복주머니 황금돼지\n 돈많은 갑부, 돈으로 할 것이 없어 무기로 쓰는 중이다."; break;

            // 20우두머리
            case MiniGamePlayerState.Guv: _Explain.text = "우두머리 돼지\n 노란돼지들의 우두머리, 였으나 일족이 멸망하고 삶이 힘들어 당근을 흔드는 중"; break;

            // 21햄스터
            case MiniGamePlayerState.Hamster: _Explain.text = "미스터 데인져러스\n 아주 위험한 햄스터, 그의 바늘공격은 상상을 초월하는 파괴력을 가지고있다."; break;

            // 22돼지집
            case MiniGamePlayerState.House: _Explain.text = "아기돼지 집\n 아기돼지들의 막내가 지었다는 집, 가끔씩 다리가 엉켜 자주 어퍼진다고 한다."; break;

            // 23백인분
            case MiniGamePlayerState.Hundred: _Explain.text = "100인분 돼지\n 족히 100명은 나누어 먹을 양의 덩치를 가진 전설의 돼지"; break;

            // 24철갑
            case MiniGamePlayerState.Iron: _Explain.text = "철갑 돼지\n 갑옷으로 자신의 약한 내면을 숨기고있는 돼지, 돌진력은 사상 최고!"; break;

            // 25돼지왕
            case MiniGamePlayerState.King: _Explain.text = "돼지왕\n 왕이긴 하나 신하가 5마리밖에 없다."; break;

            // 26마법요정
            case MiniGamePlayerState.Magic: _Explain.text = "마법요정 돼지\n 아주 오래전 대마법사[쇽]의 제자로 밑에 있을 뻔 하였다. "; break;

            // 27P
            case MiniGamePlayerState.P: _Explain.text = "Mr.P\n 모든 사건의 주범, 인간을 증오하고 멸시하는 하얀아기돼지"; break;

            // 28진주
            case MiniGamePlayerState.Pearl: _Explain.text = "돼지목에 진주 목걸이\n 진주목걸이를 하고 있는 고풍스러운 돼지, 돼지주제에 멋쟁이이다."; break;

            // 29뚱딴지
            case MiniGamePlayerState.Potato: _Explain.text = "뚱딴지\n 뚱딴지, 혹은 돼지감자 불린다. 등에 업은 꽃은 돼지감자꽃이다."; break;

            // 30구르는애
            case MiniGamePlayerState.Rolling: _Explain.text = "떼굴떼굴떼구르르\n '넌 뭐가 될거니?' 전 커서 소세지가 될래요!'"; break;

            // 31하수인
            case MiniGamePlayerState.Servant: _Explain.text = "돼지왕의 하수인\n 하수인 주제에 제일 멋쟁이다. 돼지왕의 몇 안돼는 신하"; break;

            // 32오물돼지 진
            case MiniGamePlayerState.Sludge: _Explain.text = "오물돼지(진)\n 더욱 더 더러워져 돌아온 오물돼지, 같은 돼지들도 그를 기피한다..!"; break;

            // 33오물
            case MiniGamePlayerState.Soil: _Explain.text = "오물돼지\n 오래전 여자친구에게 너무 깨끗하다면서 차인뒤 몸에 오물을 뿌리기 시작"; break;

            // 34삼형제
            case MiniGamePlayerState.Three: _Explain.text = "아기돼지 삼형제\n 서로 손을 꼭잡고 있는, 보면 기분이 좋아지는 돼지형제들"; break;

            // 35 묶인
            case MiniGamePlayerState.Tied: _Explain.text = "묶인 돼지\n 잡아먹히기 일보직전에 도망쳐나온 돼지"; break;

            // 36 돼지
            case MiniGamePlayerState.Pig: _Explain.text = "돼지\n MR.P의 계략에 두발로 일어서게 된 반혁의 초석이 된 돼지"; break;

            default: _Explain.text = "뭐냐"; break;
        }
    }


    // 선택창 선택 버튼 설정
    private void ChooseBtnUpdate()
    {
        _Button._State = _State;
    }

}
