# Project_Accepted-

it's a game "accepted" repository

1. unity version : 2018.3.14f1 (꼭 이걸로 할 것)

2. client ID : 994484244486-70sdlmmqitlqrog78oo8turkbr1k5fv1.apps.googleusercontent.com (이건 필요할 시에. gpgid 관련이여서 아마 필요없을 듯)

3. 안드로이드 스튜디오가 있으면 최소 Android 6.0 (Mashmallow)까지 다운. 다른 설정은 나중에 이미지로 따로 보내주겠음. 가급적 google play 관련은 다 다운받으면 됨.

4. android build tool : 현재 28.0.3 & 29.0.2로 설정되어 있음.

5. (유니티에서) unity -> preference -> external tools -> ndk 다운로드 (r16b 버젼 "android-ndk-r16b")

6. unity -> preference -> external tools -> use embedded JDK 체크

7. 자바 환경변수 문제가 발생시 링크 참고 : (맥 버젼) https://whitepaek.tistory.com/28    (윈도우 버젼) https://commin.tistory.com/4

8. build setting -> player setting -> android -> other settings -> Use exist Keystore 체크 & Browse keystore에서 user.keystore 선택 & keystore password : otk30dmg5c & 밑의 Key -> Alias : acceptedkey & Password : otk30dmg5c 

9. build failed가 혹시 발생할 때 Assets -> Play service resolver -> android resolver -> force resolve 하고 다시 시도





해야할 것

1. 현재 게임 실행시 강제로 구글 로그인이 되도록 해놨음. 지금 개발 단계라 ui를 꺼놨는데 구글 로그인해서 사용자의 구글 드라이브에 접속해서 맵 정보를 저장하고 불러오는 식으로 만들어놨는데 원래는 맵 정보를 받아서 맵 해금하는 방식. 여튼 이건 내가 원하는 게 아니고 버튼을 눌렀을 때 구글 로그인이 되고 맵에 대한 정보를 드라이브에서 가져오는 거고, 사용자가 이를 원치 않으면 local에 저장하는 방식으로 하는 것을 원함. 구글 로그인 시 맵 정보 저장하는 건 이미 했으니까 비로그인 시에 로컬에 저장하고, 이 정보를 구글 로그인을하면 동기화하는 것을 해줘야함.

2. 현재 undo를 대충 만들어서 개판인데, 지금 기능은 그냥 무시하고 각 오브젝트에 objectstatus 같은 스크립트가 붙어있음. 거기에 방향키 버튼을 누르기 전의 transform.position을 스택에 저장해놓고, undo를 눌렀을 때 스택에서 빼고 이전 위치로 돌아가는 방식으로 해야됨. 이쪽이 연산이 많을 거 같은데 2d게임이라 연산이 많지는 않을 듯. 이건 시간 되면 내가 만들어도 되니까 천천히 해도됨. --------------------> 해결




자잘한 오류가 워낙 많아서 다른 문제가 발생하면 그때 확인해서 해결해봐야할 듯














혹시라도 이 프로젝트를 이어간다면..




1.build시 sdk나 ndk 문제는 본인의 pc에 맞게 설정되어있어서 재다운로드해서 사용하길 바람

2.keystore key : otk30dmg5c

3.sample download : https://drive.google.com/drive/folders/1BvacPWqT9N90Nc2wEyZTMNfkrHwHE1NU?usp=sharing
